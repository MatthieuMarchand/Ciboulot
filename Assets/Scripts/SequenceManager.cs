using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (currentSequence == null)
        {
            currentSequence = sequences[0];
        }

        SetButtonsChoices();
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

    private void SetButtonsChoices()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ButtonBehavior buttonBehavior = buttons[i].GetComponent<ButtonBehavior>();
            if (buttonBehavior != null)
            {
                buttonBehavior.SetChoice(currentSequence.GetComponent<Sequence>().GetFirstChoices()[i]);
                Debug.Log("Button behavior found");
            }
            else
                Debug.Log("ButtonBehaviorNotFound");
           // Text textButton = buttons[i].GetComponentInChildren<Text>(); Texte du bouton
           // textButton.text = currentSequence.GetComponent<Sequence>().GetFirstChoices()[i].GetComponent<Choice>().text;  Texte à remplir
        }
    }
}
