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
        
        var samples = ConvertAudioData(audioData);

        return CreateAudioClip(samples);
    }

    private AudioClip CreateAudioClip(float[] samples)
    {
        AudioClip clip = AudioClip.Create("AnimaleseClip", samples.Length, 1, 44100, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private float[] ConvertAudioData(Jint.Native.JsValue audioData)
    {
        // Vérifier si le résultat est un Uint8Array (en tant qu'objet JS)
        if (!audioData.IsObject())
        {
            Debug.LogError("Les données audio ne sont pas un objet valide.");
            return null;
        }

        var jsObject = audioData.AsObject();
        int length = (int)jsObject.Get("length").AsNumber(); // Obtenir la longueur du tableau

        // Créer un tableau de float pour stocker les données audio converties
        float[] samples = new float[length];

        // Parcourir le tableau et convertir les valeurs
        for (int i = 0; i < length; i++)
        {
            var value = jsObject.Get(i.ToString()); // Accéder à l'élément indexé
            if (!value.IsArray())
            {
                Debug.LogWarning($"L'élément à l'index {i} n'est pas un nombre.");
                continue;
            }

            // Normaliser la valeur de 0-255 à -1.0 à 1.0
            samples[i] = (float)value.AsNumber() / 128.0f - 1.0f;
        }

        return samples;
    }
}
