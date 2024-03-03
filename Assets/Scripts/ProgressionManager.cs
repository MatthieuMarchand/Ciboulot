using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [FormerlySerializedAs("_sequenceManager")] [SerializeField] private SequenceManager sequenceManager;

    [SerializeField] private GameObject choiceProgressionbar;
    [FormerlySerializedAs("sequenceProgressionbar")] [SerializeField] private GameObject gameProgressionbar;
    [SerializeField] private GameObject gameProgressionBar;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        }
        if (sequenceManager == null)
        {
            sequenceManager = GameObject.FindWithTag("SequenceManager").GetComponent<SequenceManager>();
        }
        if (choiceProgressionbar == null)
        {
            choiceProgressionbar = GameObject.FindWithTag("ChoiceProgressionBar");
        }
        if (gameProgressionbar == null)
        {
            gameProgressionbar = GameObject.FindWithTag("GameProgressionBar");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sequenceManager.startChoiceStep.AddListener(SetUpChoiceProgressBar);
        gameManager.startNewSequence.AddListener(SetUpSequenceProgressBar);
    }

    private void SetUpChoiceProgressBar()
    {
        Debug.Log("Set progress bar");
        for (int i = 0; i < 3; i++)
        {
            choiceProgressionbar.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = i == sequenceManager.GetPlayerResponses().Length;
        }
    }
    
    private void SetUpSequenceProgressBar()
    {
        Debug.Log("Set Global progress bar");
        for (int i = 0; i < 5; i++)
        {
            gameProgressionbar.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = i == gameManager.GetSequenceNumber();
        }
    }
}
