using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip currentClip;
    [FormerlySerializedAs("soundSource")] [SerializeField] private AudioSource dialogueSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = .2f;

    [SerializeField] private AudioClip[] musics;

    public UnityEvent onDialoguesLoaded;

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

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        musicSource.Stop();
        if (scene.name == "HomeScreen")
        {
            musicSource.clip = musics[0];
        }
        else if (scene.name == "GameScene")
        {
            musicSource.clip = musics[1];
        }
        musicSource.Play();

    }

    //Play a animalese dialogue
    public async void PlayAnimaleseDialogueFromText(string text, bool isBoss)
    {
        if (text.Length == 0)
        {
            return;
        }

        float pitch = isBoss ? .5f : 1.3f;
        AudioClip clip = await AnimaleseManager.Instance.TextToSpeechAsync(text, false, pitch);
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
