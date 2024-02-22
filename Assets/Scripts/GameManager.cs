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
    [SerializeField] private UnityEvent LoseGame;
    [SerializeField] private UnityEvent StartNewSequence;
    

    // Start is called before the first frame update
    void Start()
    {
        if (_sequenceManager == null)
        {
            _sequenceManager = FindAnyObjectByType<SequenceManager>().GetComponent<SequenceManager>();
        }
        
        _sequenceManager.endSequence.AddListener(OnSequenceEnd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RemoveLife()
    {
        lives -= 1;
    }

    
    void OnSequenceEnd()
    {
        if (lives < 1)
        {
            LoseGame.Invoke();
        }
        else
        {
            sequenceNumber += 1;
            StartNewSequence.Invoke();
        }
    }
}
