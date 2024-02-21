using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int lives;
    [SerializeField] private int sequenceNumber = 0;
    [SerializeField] private UnityEvent LoseGame;
    [SerializeField] private SequenceManager _sequenceManager;
    

    // Start is called before the first frame update
    void Start()
    {
        if (_sequenceManager == null)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RemoveLife()
    {
        lives -= 1;
    }

    
    void CheckGameState()
    {
        if (lives < 1)
        {
            LoseGame.Invoke();
        }
    }
}
