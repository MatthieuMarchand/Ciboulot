using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip currentClip;
    [FormerlySerializedAs("soundSource")] [SerializeField] private AudioSource dialogueSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = .2f;

    public static SoundManager Instance { get; private set; }

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

    //Play a animalese dialogue
    public void PlayAnimaleseDialogueFromText(string text, bool isBoss)
    {
        if (text.Length == 0)
        {
            return;
        }

        float pitch = isBoss ? .5f : 1.3f;
        AudioClip clip = AnimaleseManager.Instance.TextToSpeech(text, false, pitch);
        if (!clip || !dialogueSource || !musicSource)
        {
            return;
        }
        dialogueSource.Stop();
        PlayDialogueWithFade(clip);
    }

    private void PlayDialogueWithFade(AudioClip clip)
    {
        musicSource.DOFade(.3f, fadeDuration).OnComplete(() =>
        {
            dialogueSource.clip = clip;
            dialogueSource.Play();
            Invoke(nameof(RestoreMusicVolume), clip.length + .1f); //Adding .1f to avoid errors.
        });
    }

    private void RestoreMusicVolume()
    {
        if (dialogueSource.isPlaying)
        {
            return;
        }
        musicSource.DOFade(1f, fadeDuration);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
