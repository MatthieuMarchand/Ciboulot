using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int fingers;
    [SerializeField] int score;
    const int WIN_SCORE = 5;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public void WinSequence()
    {
        score++;
        if (score == WIN_SCORE) { }
        //Appelle WinScript de fin
        else {
            
            StartSequence();
        }
    }

    public void LoseSequence()
    {
        if (fingers == 0) {
            //loseScript
        }
        else
            StartSequence();
    }

    public void StartSequence()
    {
        
    }
}
