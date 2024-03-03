using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class UIManager : MonoBehaviour
{
    public enum UIType
    {
        SetStartGame,
        SetIssue,
        SetChoice,
        SetEndChoice,
        SetEndSequence,
        SetIntroOrEndGame
    }
    
    [SerializeField] private GameObject issueContainer;
    
    [SerializeField] private GameObject choiceContainer;
    [SerializeField] private GameObject choice1Container;
    [SerializeField] private GameObject choice2Container;
    [SerializeField] private GameObject choice3Container;
    [SerializeField] private GameObject recapChoiceContainer;
    
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SequenceManager sequenceManager;

    
    
    [FormerlySerializedAs("TimerUI")] [SerializeField] private GameObject timerUI;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        }
        if (sequenceManager == null)
        {
            sequenceManager = FindAnyObjectByType<SequenceManager>().GetComponent<SequenceManager>();
        }
        
        gameManager.startIntro.AddListener(SetUpIntroUI);
        //gameManager.startNewSequence.AddListener(); //TODO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        if (issueContainer && timerUI && choiceContainer && choice1Container && choice2Container && choice3Container &&
            recapChoiceContainer) return;
        issueContainer = GameObject.FindGameObjectWithTag("IssueContainer");
        choiceContainer = GameObject.FindGameObjectWithTag("ChoiceContainer");
        timerUI = GameObject.FindGameObjectWithTag("Timer").gameObject;
        choice1Container = choiceContainer.transform.Find("choix1").gameObject;
        choice2Container = choiceContainer.transform.Find("choix2").gameObject;
        choice3Container = choiceContainer.transform.Find("choix3").gameObject;
        recapChoiceContainer = choiceContainer.transform.Find("AllChoice").gameObject;
    }

    private void SetUpIntroUI()
    {
        SetUpUI(UIManager.UIType.SetStartGame);
    }
    
    public void SetUpUI(Enum UItype, string newText1 = "", string newText2 = "", string newText3 = "")
    {
        switch (UItype)
        {
            case UIType.SetStartGame:
                choiceContainer.SetActive(false);
                timerUI.SetActive(false);
                break;
            case UIType.SetIssue:
                choiceContainer.SetActive(false);
                issueContainer.SetActive(true);
                timerUI.SetActive(true);
                SetIssueText(newText1, newText2);
                break;
            case UIType.SetChoice:
                timerUI.SetActive(true);
                issueContainer.SetActive(false);
                choiceContainer.SetActive(true);
                choice1Container.SetActive(true);
                choice2Container.SetActive(true);
                choice3Container.SetActive(true);
                SetChoicesText(newText1, newText2, newText3);
                break;
            case UIType.SetEndChoice:
                issueContainer.SetActive(false);
                timerUI.SetActive(false);
                choice1Container.SetActive(false);
                choice2Container.SetActive(false);
                choice3Container.SetActive(false);
                break;
        }
    }

    private void SetIssueText(string questionPart1, string questionPart2)
    {
        for (int i = 0; i < issueContainer.transform.childCount; i++)
        {
            if (issueContainer.transform.GetChild(i).gameObject.name == "Question 1")
            {
                issueContainer.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    questionPart1;
            }
            else if (issueContainer.transform.GetChild(i).gameObject.name == "Question 2")
            {
                issueContainer.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    questionPart2;
            }
                
        }
    }
    private void SetChoicesText(string choiceText1, string choiceText2, string choiceText3)
    {
        choiceContainer.transform.Find("choix1").gameObject.transform.GetChild(0).gameObject
                .GetComponentInChildren<TextMeshProUGUI>().text =
            choiceText1;
        choiceContainer.transform.Find("choix2").gameObject.transform.GetChild(0).gameObject
                .GetComponentInChildren<TextMeshProUGUI>().text =
            choiceText2;
        choiceContainer.transform.Find("choix3").gameObject.transform.GetChild(0).gameObject
                .GetComponentInChildren<TextMeshProUGUI>().text =
            choiceText3;
    }

}
