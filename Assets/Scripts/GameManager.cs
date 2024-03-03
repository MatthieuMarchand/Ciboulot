using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    [SerializeField] private SequenceManager _sequenceManager;
    [SerializeField] private AnimationManager _animationManager;
    
    
    public UnityEvent loseGame;
    public UnityEvent winGame;
    public UnityEvent startNewSequence;
    public BoolEvent checkGameState;
    public UnityEvent endScreen;

    [FormerlySerializedAs("startGame")] public UnityEvent startIntro;
    
//Getter and setter

    public int GetSequenceNumber()
    {
        return sequenceNumber;
    }

    private void Awake()
    {
        if (_sequenceManager == null)
        {
            _sequenceManager = FindAnyObjectByType<SequenceManager>().GetComponent<SequenceManager>();
        }
        if (_animationManager == null)
        {
            _animationManager = GameObject.FindWithTag("AnimationManager").GetComponent<AnimationManager>();
        }

        checkGameState = new BoolEvent();

        
        loseGame.AddListener(OnLoseGame);
        winGame.AddListener(WinGameHandler);
        startNewSequence.AddListener(StartNewSequenceHandler);
        startIntro.AddListener(StartIntroHandler);
        endScreen.AddListener(SwitchToEndScreen);

    }

    private void Start()
    {
        _animationManager.introIsOver.AddListener(StartGameHandler);
        checkGameState.AddListener(OnSequenceEnd);
        startIntro.Invoke();
    }


    private void RemoveLife()
    {
        lives -= 1;
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
            if (sequenceNumber == _sequenceManager.GetFirstIssues().Length)
                winGame.Invoke();
            else
                startNewSequence.Invoke();
        }
    }
    
    // Handlers pour les événements
    private void OnLoseGame()
    {
        Debug.Log("Game Over");
    }

    private void WinGameHandler()
    {
        Debug.Log("You Win!");
    }

    private void StartNewSequenceHandler()
    {
        Debug.Log("Starting new sequence...");
    }
    
    private void StartIntroHandler()
    {
        Debug.Log("Starting Intro...");
        
    }
    private void StartGameHandler()
    {
        Debug.Log("Starting Game...");
        startNewSequence.Invoke();
    }
    
    private void SwitchToEndScreen()
    {
        Debug.Log("EndScreen...");
        SceneManager.LoadScene("WinScreen");
    }
}
