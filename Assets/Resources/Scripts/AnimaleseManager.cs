using UnityEngine;
using Jint;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Serialization;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AnimaleseManager : MonoBehaviour
{
    [FormerlySerializedAs("animalseWavFile")] [SerializeField]
    private AudioClip animaleseWavFile;
    private Engine jsEngine;
    
    private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();
    private const int MAX_CACHE_SIZE = 100; // cache limit
    private Queue<string> cacheOrder = new Queue<string>();
    public bool isIntitialized { get; private set; }

    public UnityEvent onInitialized;
    
    public static AnimaleseManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        try
        {
            jsEngine = new Engine();

            TextAsset animaleseJs = Resources.Load<TextAsset>("Scripts/animalese");
            if (animaleseJs == null)
            {
                Debug.LogError("Cannot load animalese.txt!");
                return;
            }
        
            string animaleseJsCode = animaleseJs.text;
            jsEngine.SetValue("log", new Action<object>(Debug.Log));
        
            InitializeAnimalese(animaleseJsCode);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error initializing Animalese: {e}");
        }
    }

    private void InitializeAnimalese(string jsCode)
    {
        if (animaleseWavFile == null)
        {
            Debug.LogError("Wav file unassigned !");
            return;
        }

        // Convert .wav clip into raw data.
        float[] samples = new float[animaleseWavFile.samples];
        animaleseWavFile.GetData(samples, 0);
        
        byte[] letterLibrary = new byte[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            float sample = samples[i];
            sample = (sample + 1f) * 127.5f;
            letterLibrary[i] = (byte)Mathf.Clamp(sample, 0f, 255f);
        }
        
        jsEngine.Execute(jsCode);
        
        // Create Animalese instance.
        jsEngine.Execute("var animalese = new Animalese();");
        
        // Create letterLibrary
        jsEngine.Execute($"var letterLibraryData = new Uint8Array({letterLibrary.Length});");
        for (int i = 0; i < letterLibrary.Length; i++)
        {
            jsEngine.Execute($"letterLibraryData[{i}] = {letterLibrary[i]};");
        }
        jsEngine.Execute("animalese.letter_library = letterLibraryData;");
        
        isIntitialized = true;
        onInitialized.Invoke();
    }

    public AudioClip TextToSpeech(string text, bool shorten = true, float pitch = 1.0f)
    {
        string cacheKey = $"{text}_{pitch}";
        if (audioCache.TryGetValue(cacheKey, out AudioClip cachedClip))
        {
            return cachedClip;
        }
        
        string escapedText = text.Replace("'", "\\'")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r");

        AudioClip clip = GenerateAudioClip(escapedText, shorten, pitch);
        if (!clip)
        {
            return null;
        }
        
        AddToCache(cacheKey, clip);
        
        return clip;
    }
    
    //Returns an Audioclip and cache it.
    public async Task<AudioClip> TextToSpeechAsync(string text, bool shorten = true, float pitch = 1.0f)
    {
        string cacheKey = $"{text}_{pitch}";

        if (audioCache.TryGetValue(cacheKey, out AudioClip cachedClip))
        {
            return cachedClip;
        }
        
        string escapedText = text.Replace("'", "\\'")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r");

        // Generate audio on seperated thread
        var audioData = await Task.Run(() => 
        {
            if (jsEngine == null)
            {
                Debug.LogError("JsEngine is null");
                return null;
            }
            var result = jsEngine.Evaluate($"animalese.Animalese('{escapedText}', {shorten.ToString().ToLower()}, {pitch})");
            return result.ToObject() as object[];
        });

        // Get back on main thread to create AudioClip
        AudioClip clip = null;
        await Task.Run(() =>
        {
            var samples = new float[audioData.Length];
            for (int i = 0; i < audioData.Length; i++)
            {
                float sample = Convert.ToSingle(audioData[i]);
                samples[i] = (sample - 127f) / 128f;
            }

            // Create audio clip on main thread
            MainThreadDispatcher.Instance.Enqueue(() =>
            {
                if (samples.Length < 1)
                {
                    return;
                }
                clip = AudioClip.Create("AnimalSpeak", samples.Length, 1, 44100, false);
                clip.SetData(samples, 0);
                
                // Ajouter au cache
                AddToCache(cacheKey, clip);
            });
        });

        return clip;
    }

    public AudioClip GenerateAudioClip(string text, bool shorten, float pitch)
    {
        if (text.Length > 50)
        {
            text = text.Remove(50);
        }
        try
        {
            // Get raw data from animalese.js
            var result = jsEngine.Evaluate($"animalese.Animalese('{text}', {shorten.ToString().ToLower()}, {pitch})");
            var audioData = result.ToObject() as object[];
            
            // Create audio clip
            var samples = new float[audioData.Length];
            for (int i = 0; i < audioData.Length; i++)
            {
                float sample = Convert.ToSingle(audioData[i]);
                samples[i] = (sample - 127f) / 128f;
            }
            
            AudioClip clip = AudioClip.Create("AnimalSpeak", samples.Length, 1, 44100, false);
            clip.SetData(samples, 0);
            
            return clip;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error when generating voice : {e.Message}");
            return null;
        }
    }

    private void AddToCache(string key, AudioClip clip)
    {
        if (audioCache.Count >= MAX_CACHE_SIZE)
        {
            string oldestKey = cacheOrder.Dequeue();
            audioCache.Remove(oldestKey);
        }

        audioCache[key] = clip;
        cacheOrder.Enqueue(key);
    }

    private void OnDestroy()
    {
        if (jsEngine == null) return;
        jsEngine.Dispose();
        jsEngine = null;
    }
}