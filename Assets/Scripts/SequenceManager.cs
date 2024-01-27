using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    [SerializeField] GameObject currentSequence;
    [SerializeField] GameObject[] sequences;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject[] playerResponses;
    float timer = 0;


    [SerializeField] GameManager gameManager;

    public float GetTimer()
    {
        return timer;
    }
    public void SetTimer(float newTimer)
    {
        timer = newTimer;
    }
    public void IncreaseTimer()
    {
        timer += 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void validSequence(GameObject choice)
    {
        List<GameObject> listeResponses = new List<GameObject>(playerResponses);

        // Ajouter un élément à la liste
        listeResponses.Add(choice);

        // Reconvertir la liste en tableau
        playerResponses = listeResponses.ToArray();


    }

    public void CheckChoices()
    {
        int badAnswers = 0;
        for (int i = 0; i < playerResponses.Length; i++)
        {
            if (playerResponses[i] == currentSequence.GetComponent<Sequence>().GetValidChoices()[i])
            {
                badAnswers++;
            }
        }

        if (badAnswers > 1)
        {
            gameManager.LoseSequence();
        }
        else
            gameManager.WinSequence();
    }
}
