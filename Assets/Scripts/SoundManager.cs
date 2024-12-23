using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip currentClip;
    [FormerlySerializedAs("soundSource")] [SerializeField] private AudioSource dialogueSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = .2f;
    public event Action OnDialogueEnd;

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

    public void PlaySound(AudioClip clip)
    {
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
            Invoke(nameof(TriggerOnDialogueEnd), clip.length);
        });
    }

    private void TriggerOnDialogueEnd()
    {
        OnDialogueEnd?.Invoke();
    }

    private void Start()
    {
        OnDialogueEnd += RestoreMusicVolume;
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
        OnDialogueEnd -= RestoreMusicVolume;
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
