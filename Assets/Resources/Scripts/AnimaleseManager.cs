using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System.Threading.Tasks;
using UnityEngine.Events;

public class AnimaleseManager : MonoBehaviour
{
    [FormerlySerializedAs("animalseWavFile")] [SerializeField]
    private AudioClip animaleseWavFile;
    
    //cache
    private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();
    private const int MAX_CACHE_SIZE = 100; // cache limit
    private Queue<string> cacheOrder = new Queue<string>();
    
    //animalese converter
    private const int SAMPLE_FREQUENCY = 44100;
    private const float LIBRARY_LETTER_SECS = 0.15f;
    private const float OUTPUT_LETTER_SECS = 0.075f;
    private byte[] letterLibrary;
    
    public bool isInitialized { get; private set; }
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
    }

    void Start()
    {
        InitializeAnimalese();
    }

    private void InitializeAnimalese()
    {
        if (animaleseWavFile == null)
        {
            Debug.LogError("Wav file unassigned!");
            return;
        }

        // Convert .wav clip into raw data
        float[] samples = new float[animaleseWavFile.samples];
        animaleseWavFile.GetData(samples, 0);
        
        letterLibrary = new byte[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            float sample = samples[i];
            sample = (sample + 1f) * 127.5f;
            letterLibrary[i] = (byte)Mathf.Clamp(sample, 0f, 255f);
        }
        
        isInitialized = true;
        onInitialized.Invoke();
    }
    
    //generate audio samples data on seperated thread, returns an Audioclip created from it and cache it.
    public async Task<AudioClip> TextToSpeechAsync(string text, bool shorten = true, float pitch = 1.0f)
    {
        string cacheKey = $"{text}_{pitch}";

        if (audioCache.TryGetValue(cacheKey, out AudioClip cachedClip))
        {
            return cachedClip;
        }

        // Generate audio on seperated thread
        var samples = await Task.Run(() => GenerateAnimalese(text, shorten, pitch));

        AudioClip clip = null;

        // Create audio clip on main thread
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            if (samples.Length < 1)
            {
                return;
            }
            clip = AudioClip.Create("AnimalSpeak", samples.Length, 1, 44100, false);
            clip.SetData(samples, 0);
            
            AddToCache(cacheKey, clip);
        });

        return clip;
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

    private float[] GenerateAnimalese(string script, bool shorten, float pitch)
    {
        string processedScript = ProcessScript(script, shorten);
        
        int librarySamplesPerLetter = Mathf.FloorToInt(LIBRARY_LETTER_SECS * SAMPLE_FREQUENCY);
        int outputSamplesPerLetter = Mathf.FloorToInt(OUTPUT_LETTER_SECS * SAMPLE_FREQUENCY);
        float[] data = new float[processedScript.Length * outputSamplesPerLetter];
        
        for (int cIndex = 0; cIndex < processedScript.Length; cIndex++)
        {
            char c = char.ToUpper(processedScript[cIndex]);
            
            if (c >= 'A' && c <= 'Z')
            {
                int libraryLetterStart = librarySamplesPerLetter * (c - 'A');
                
                for (int i = 0; i < outputSamplesPerLetter; i++)
                {
                    int sampleIndex = 44 + libraryLetterStart + Mathf.FloorToInt(i * pitch);
                    if (sampleIndex < letterLibrary.Length)
                    {
                        data[cIndex * outputSamplesPerLetter + i] = (letterLibrary[sampleIndex] - 127f) / 128f;
                    }
                }
            }
            else
            {
                // Non-pronounceable character or space
                for (int i = 0; i < outputSamplesPerLetter; i++)
                {
                    data[cIndex * outputSamplesPerLetter + i] = 0f; // Equivalent to 127 in the original (silence)
                }
            }
        }
        
        return data;
    }

    private string ShortenWord(string word)
    {
        if (word.Length > 1)
        {
            return $"{word[0]}{word[word.Length - 1]}";
        }
        return word;
    }
    
    private string ProcessScript(string script, bool shorten)
    {
        if (!shorten) return script;
        
        // Split into words, remove non-letters, and shorten each word
        var words = System.Text.RegularExpressions.Regex.Replace(script, @"[^a-zA-Z]", " ")
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
        return string.Join("", Array.ConvertAll(words, ShortenWord));
    }
}