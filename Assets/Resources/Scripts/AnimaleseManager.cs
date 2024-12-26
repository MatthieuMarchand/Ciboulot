using UnityEngine;
using Jint;
using System;
using System.IO;
using UnityEngine.Serialization;

public class AnimaleseManager : MonoBehaviour
{
    [FormerlySerializedAs("animalseWavFile")] [SerializeField]
    private AudioClip animaleseWavFile;
    private Engine jsEngine;
    
    public static AnimaleseManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        jsEngine = new Engine();

        string animaleseJsPath = Path.Combine(Application.dataPath, "Resources/Scripts/animalese.js");
        string animaleseJsCode = File.ReadAllText(animaleseJsPath);
        
        jsEngine.SetValue("log", new Action<object>(Debug.Log));
        
        InitializeAnimalese(animaleseJsCode);
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
    }

    public AudioClip TextToSpeech(string text, bool shorten = true, float pitch = 1.0f)
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
}