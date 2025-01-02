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
    public bool isLoading { get; private set; } = false;
    
    public static LoadingBehaviour Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public async Task LoadDialogues(Dictionary<string, bool> dialogues)
    {
        if (loadingScreen)
        {
            loadingScreen.SetActive(true);
        }
        
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

        UpdateLoadingProgress(1f, "Chargement terminé!");
        
        await Task.Delay(500);

        if (!loadingScreen)
        {
            return;
        }
        loadingScreen.SetActive(false);
        isLoading = false;
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
