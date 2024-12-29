using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public class BoolEvent : UnityEvent<bool>
    {
    }
    
    [SerializeField] private int lives;
    [SerializeField] private int sequenceNumber = 0;
    [FormerlySerializedAs("_sequenceManager")] [SerializeField] private SequenceManager sequenceManager;
    [FormerlySerializedAs("_animationManager")] [SerializeField] private AnimationManager animationManager;
    
    
    public UnityEvent loseGame;
    public UnityEvent winGame;
    public UnityEvent startNewSequence;
    public BoolEvent checkGameState;
    public UnityEvent endScreen;
    public UnityEvent lifeRemoved;
    
    public static GameManager Instance { get; private set; }
    
    [FormerlySerializedAs("startGame")] public UnityEvent startIntro;
    
//Getter and setter

    public int GetSequenceNumber()
    {
        return sequenceNumber;
    }

    public int GetLives()
    {
        return lives;
    }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; 
        DontDestroyOnLoad(gameObject);
        
        if (sequenceManager == null)
        {
            sequenceManager = FindAnyObjectByType<SequenceManager>().GetComponent<SequenceManager>();
        }
        if (animationManager == null)
        {
            animationManager = GameObject.FindWithTag("AnimationManager").GetComponent<AnimationManager>();
        }

        checkGameState = new BoolEvent();
        
        loseGame.AddListener(OnLoseGame);
        winGame.AddListener(WinGameHandler);
        startNewSequence.AddListener(StartNewSequenceHandler);
        startIntro.AddListener(StartIntroHandler);
        endScreen.AddListener(SwitchToEndScreen);
    }

    private async void Start()
    {
        animationManager.introIsOver.AddListener(StartGameHandler);
        checkGameState.AddListener(OnSequenceEnd);
        if (!LoadingBehaviour.Instance || !SequenceManager.Instance || !AnimaleseManager.Instance)
        {
            return;
        }

        var dialogues = SequenceManager.Instance.GetAllTexts();
        if (!AnimaleseManager.Instance.isIntitialized)
        {
            //wait for AnimaleseManager to initialize
            var tcs = new TaskCompletionSource<bool>();
        
            AnimaleseManager.Instance.onInitialized.AddListener(() =>
            {
                tcs.SetResult(true);
            });

            await tcs.Task;
        }
        await LoadingBehaviour.Instance.LoadDialogues(dialogues);
        startIntro.Invoke();
    }


    private void RemoveLife()
    {
        lives -= 1;
        lifeRemoved.Invoke();
    }


    private void OnSequenceEnd(bool goodSequence)
    {
        if (!goodSequence)
            RemoveLife();
        
        if (lives < 1)
        {
            loseGame.Invoke();
        }
        else
        {
            sequenceNumber ++;
            if (sequenceNumber == sequenceManager.GetIssues().Length)
                winGame.Invoke();
            else
                startNewSequence.Invoke();
        }
    }
    
    // Handlers pour les événements
    private static void OnLoseGame()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("LoseScreen");
    }

    private static void WinGameHandler()
    {
        Debug.Log("You Win!");
    }

    private static void StartNewSequenceHandler()
    {
        Debug.Log("Starting new sequence...");
    }
    
    private static void StartIntroHandler()
    {
        Debug.Log("Starting Intro...");
        
    }
    public void StartGameHandler()
    {
        Debug.Log("Starting Game...");
        GameObject.FindWithTag("StartButton").SetActive(false);
        startNewSequence.Invoke();
    }
    
    private static void SwitchToEndScreen()
    {
        Debug.Log("EndScreen...");
        SceneManager.LoadScene("WinScreen");
    }
}
