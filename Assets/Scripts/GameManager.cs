using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int lives;
    [SerializeField] private int sequenceNumber = 0;
    [SerializeField] private SequenceManager _sequenceManager;
    [SerializeField] private AnimationManager _animationManager;
    
    
    public UnityEvent loseGame;
    public UnityEvent winGame;
    public UnityEvent startNewSequence;
    public UnityEvent checkGameState;

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
        
        loseGame.AddListener(OnLoseGame);
        winGame.AddListener(WinGameHandler);
        startNewSequence.AddListener(StartNewSequenceHandler);
        startIntro.AddListener(StartIntroHandler);
    }

    void Start()
    {
        _animationManager.introIsOver.AddListener(StartGameHandler);
        _sequenceManager.EndSequenceStep.AddListener(OnSequenceEnd);
        startIntro.Invoke();
    }


    void RemoveLife()
    {
        lives -= 1;
    }

    
    void OnSequenceEnd(bool goodSequence)
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
}
