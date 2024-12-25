using System;
using UnityEngine;
using Jint;
using System.IO;
using System.Linq;
using Jint.Native.Object;

public class AnimaleseManager : MonoBehaviour
{
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

        string animaleseJsPath = Path.Combine(Application.dataPath + "/Resources/Scripts/animalese.js");
        string animaleseJsCode = File.ReadAllText(animaleseJsPath);
        
        jsEngine.SetValue("log", new Action<object>(Debug.Log));
        jsEngine.Execute(animaleseJsCode);
//          jsEngine.Execute(@"
//              var synth = new Animalese('animalese.wav', function() {
//                  log('Animalese initialisé');
//              });
//
//              //synth.Animalese('Hello world', false, 1.0);
//          ");
    }

    public AudioClip GenerateAnimalese(string text)
    {
        var audioData = jsEngine.Invoke("Animalese", text, false, 1.0);

        if (audioData is not Jint.Native.JsArray)
        {
            Debug.LogWarning(audioData + " is not a JsArray");
            return null;
        }
        
        var samples = ConvertAudioData(audioData.AsArray());

        return CreateAudioClip(samples);
    }

    private AudioClip CreateAudioClip(float[] samples)
    {
        AudioClip clip = AudioClip.Create("AnimaleseClip", samples.Length, 1, 44100, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private float[] ConvertAudioData(Jint.Native.JsArray audioData)
    {
        var jsArray = audioData;
        
        float[] samples = new float[jsArray.Length];
        for (int i = 0; i < jsArray.Length; i++)
        {
            if (jsArray[i] is not Jint.Native.JsNumber)
            {
                Debug.LogWarning(jsArray[i] + " is not a JsNumber");
            }
            samples[i] = (float)jsArray[i].AsNumber();
        }
        return samples;
    }
}
