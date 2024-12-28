using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Serialization;

public class LoadingBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [FormerlySerializedAs("progressBar")] [SerializeField] private Image progressImage;
    [SerializeField] private TextMeshProUGUI progressText;
    
    public static LoadingBehaviour Instance { get; private set; }

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
    
    public async Task LoadDialogues(Dictionary<string, bool> dialogues)
    {
        loadingScreen.SetActive(true);
        
        int totalDialogues = dialogues.Count;
        int currentDialogue = 0;

        foreach (var dialogue in dialogues)
        {
            var pitch = dialogue.Value ? .5f : 1.3f;
            
            float progress = (float)currentDialogue / totalDialogues;
            UpdateLoadingProgress(progress, $"Chargement des dialogues... ({currentDialogue}/{totalDialogues})");
            
            await AnimaleseManager.Instance.TextToSpeechAsync(dialogue.Key, true, pitch);
            
            currentDialogue++;
        }

        // Mise à jour finale
        UpdateLoadingProgress(1f, "Chargement terminé!");
        
        // Attendre un peu pour montrer le 100%
        await Task.Delay(500);
        
        // Cacher l'écran de chargement
        loadingScreen.SetActive(false);
    }
    
    private void UpdateLoadingProgress(float progress, string message)
    {
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            if (progressImage != null)
                progressImage.transform.Rotate(0, 0, progressImage.transform.rotation.z - 20);
            
            if (progressText != null)
                progressText.text = message;
        });
    }
}
