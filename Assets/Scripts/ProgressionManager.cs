using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SequenceManager _sequenceManager;

    [SerializeField] private GameObject choiceProgressionbar;
    [SerializeField] private GameObject gameProgressionBar;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        }
        if (_sequenceManager == null)
        {
            _sequenceManager = GameObject.FindWithTag("SequenceManager").GetComponent<SequenceManager>();
        }
        if (choiceProgressionbar == null)
        {
            choiceProgressionbar = GameObject.FindWithTag("ChoiceProgressionBar");
        }
        if (gameProgressionBar == null)
        {
            gameProgressionBar = GameObject.FindWithTag("GameProgressionBar");
        }
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _sequenceManager.startChoiceStep.AddListener(SetUpProgressBar);
    }

    private void SetUpProgressBar()
    {
        Debug.Log("Set progress bar");
        switch (_sequenceManager.GetPlayerResponses().Length)
        {
            case 0:
                choiceProgressionbar.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                choiceProgressionbar.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                choiceProgressionbar.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                break;
            case 1:
                choiceProgressionbar.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                choiceProgressionbar.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                choiceProgressionbar.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                break;
            case 2:
                choiceProgressionbar.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                choiceProgressionbar.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                choiceProgressionbar.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
                break;
        }
        
        
    }
}
