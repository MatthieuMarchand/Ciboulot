using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SequenceManager : MonoBehaviour
{
    [SerializeField] GameObject currentSequence;
    [SerializeField] GameObject issue;
    [SerializeField] GameObject[] sequences;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject[] playerResponses;
    float timer = 0;


    [SerializeField] GameManager gameManager;

    public float GetTimer()
    {
        return timer;
    }

    public GameObject GetCurrentSequence()
    {
        return currentSequence;
    }
    public void SetCurrentSequence(int sequenceNumber)
    {
        currentSequence = sequences[sequenceNumber];
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
        DontDestroyOnLoad(gameObject);

        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        if (currentSequence == null)
        {
            currentSequence = sequences[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddChoiceToPlayerResponses(GameObject choice)
    {
        List<GameObject> listeResponses = new List<GameObject>(playerResponses);

        // Ajouter un élément à la liste
        listeResponses.Add(choice);

        // Reconvertir la liste en tableau
        playerResponses = listeResponses.ToArray();
    }

    public void CheckIfResponseIsComplete(GameObject choice)
    {
        if (playerResponses.Length == 3)
        {
            CheckChoices();
            Debug.Log("CheckChoices");
        }
        else
        {
            SetButtonsChoices(choice.GetComponent<Choice>().GetChoices());
        }

    }

    public void CheckChoices()
    {
        int badAnswers = 0;
        for (int i = 0; i < playerResponses.Length; i++)
        {
            if (playerResponses[i] != currentSequence.GetComponent<Sequence>().GetValidChoices()[i])
            {
                badAnswers++;
            }
        }

        if (badAnswers > 1)
        {
            gameManager.LoseSequence();
            Debug.Log("loseSequence");
        }
        else
            gameManager.WinSequence();

        Array.Clear(playerResponses, 0, playerResponses.Length) ;
    }

    public void SetIssueText()
    {
        string text = currentSequence.GetComponent<Sequence>().GetIssue();
        issue.GetComponentInChildren<TMP_Text>().text = text;

    }
    public void SetButtonsChoices(GameObject[] choices)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ButtonBehavior buttonBehavior = buttons[i].GetComponent<ButtonBehavior>();
            if (buttonBehavior != null)
            {
                buttonBehavior.SetChoice(choices[i]);
            }
            else
                Debug.Log("ButtonBehaviorNotFound");
           
        }
    }
}
