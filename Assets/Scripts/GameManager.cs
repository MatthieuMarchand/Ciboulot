using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int lives;
    [SerializeField] private int sequenceNumber = 0;
    [SerializeField] private SequenceManager _sequenceManager;
    public UnityEvent loseGame;
    public UnityEvent winGame;
    public UnityEvent startNewSequence;
    
//Getter and setter

    public int GetSequenceNumber()
    {
        return sequenceNumber;
    }
    
    
    void Start()
    {
        if (_sequenceManager == null)
        {
            _sequenceManager = FindAnyObjectByType<SequenceManager>().GetComponent<SequenceManager>();
        }
        
        _sequenceManager.endSequence.AddListener(OnSequenceEnd);
    }


    void RemoveLife()
    {
        lives -= 1;
    }

    
    void OnSequenceEnd()
    {
        if (lives < 1)
        {
            loseGame.Invoke();
        }
        else
        {
            sequenceNumber += 1;
            if (sequenceNumber > 4)
                winGame.Invoke();
            else
                startNewSequence.Invoke();
        }
    }
}
