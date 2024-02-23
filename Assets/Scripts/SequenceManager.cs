using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class SequenceManager : MonoBehaviour
{
    private float _timer;
    private bool isTimerActivated;
    [SerializeField] private GameObject TimerUI;
    public UnityEvent endSequence;

    [SerializeField]
    private GameObject[] playerResponses;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject CurrentIssue;
    [SerializeField] private GameObject CurrentStep;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        }
        
        gameManager.startNewSequence.AddListener(OnStartNewSequence);
    }

    void Update()
    {
        if (isTimerActivated == false)
            return;
        
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            //todo
        }
                
    }

    public void EndSequence()
    {
        endSequence.Invoke();
    }

    public void CheckResponses()
    {
        
    }

    public void CheckNumberOfResponses()
    {
        
    }

    public void OnStartNewSequence()
    {
        switch (gameManager.GetSequenceNumber())
        {
            case 1:
                
                break;
        }
        
        _timer = 5f;
        isTimerActivated = true;
    }
    
}
