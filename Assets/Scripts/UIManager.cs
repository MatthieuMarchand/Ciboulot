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
        SetEndGame,
        SetRecapPlayersResponses,
        HideRecap
    }
    
    [SerializeField] private GameObject issueContainer;
    
    [SerializeField] private GameObject choiceContainer;
    [SerializeField] private GameObject choice1Container;
    [SerializeField] private GameObject choice2Container;
    [SerializeField] private GameObject choice3Container;
    [SerializeField] private GameObject recapChoiceContainer;
    
    [SerializeField] private GameObject choiceProgressionbar;
    [SerializeField] private GameObject gameProgressionBar;
    [SerializeField] private GameObject skipButton;


    
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
        gameManager.winGame.AddListener(SetUpEndGame);
        gameManager.loseGame.AddListener(SetUpEndGame);


        if (issueContainer && timerUI && choiceContainer && choice1Container && choice2Container && choice3Container &&
            recapChoiceContainer && choiceProgressionbar && gameProgressionBar && skipButton) return;
        issueContainer = GameObject.FindGameObjectWithTag("IssueContainer");
        choiceContainer = GameObject.FindGameObjectWithTag("ChoiceContainer");
        timerUI = GameObject.FindGameObjectWithTag("Timer").gameObject;
        choice1Container = choiceContainer.transform.Find("choix1").gameObject;
        choice2Container = choiceContainer.transform.Find("choix2").gameObject;
        choice3Container = choiceContainer.transform.Find("choix3").gameObject;
        recapChoiceContainer = choiceContainer.transform.Find("AllChoice").gameObject;
        choiceProgressionbar = GameObject.FindWithTag("ChoiceProgressionBar");
        gameProgressionBar = GameObject.FindWithTag("GameProgressionBar");
        skipButton = GameObject.FindWithTag("StartButton");
    }

    private void SetUpIntroUI()
    {
        SetUpUI(UIManager.UIType.SetStartGame);
    }

    private void SetUpEndGame()
    {
        SetUpUI(UIType.SetEndGame);
    }
    
    public void SetUpUI(Enum UItype, string[] texts = null)
    {
        switch (UItype)
        {
            case UIType.SetStartGame:
                gameProgressionBar.SetActive(false);
                recapChoiceContainer.SetActive(false);
                choiceContainer.SetActive(false);
                timerUI.SetActive(false);
                skipButton.SetActive(true);
                break;
            case UIType.SetIssue:
                gameProgressionBar.SetActive(true);
                choiceContainer.SetActive(false);
                issueContainer.SetActive(true);
                timerUI.SetActive(true);
                skipButton.SetActive(true);
                SetIssueText(texts);
                break;
            case UIType.SetChoice:
                timerUI.SetActive(true);
                issueContainer.SetActive(false);
                choiceContainer.SetActive(true);
                choice1Container.SetActive(true);
                choice2Container.SetActive(true);
                choice3Container.SetActive(true);
                skipButton.SetActive(false);
                if (texts != null)
                {
                    SetChoicesText(texts[0], texts[1], texts[2]);
                }
                break;
            case UIType.SetEndChoice:
                issueContainer.SetActive(false);
                timerUI.SetActive(false);
                choice1Container.SetActive(false);
                choice2Container.SetActive(false);
                choice3Container.SetActive(false);
                skipButton.SetActive(false);
                break;
            case UIType.SetRecapPlayersResponses:
                timerUI.SetActive(false);
                issueContainer.SetActive(false);
                choice1Container.SetActive(false);
                choice2Container.SetActive(false);
                choice3Container.SetActive(false);
                recapChoiceContainer.SetActive(true);
                skipButton.SetActive(false);
                if (texts != null)
                {
                    SetRecapPlayerResponsesText(texts[0], texts[1], texts[2]);
                }
                break;
            case UIType.HideRecap:
                recapChoiceContainer.SetActive(false);
                break;
            case UIType.SetEndGame:
                gameProgressionBar.SetActive(false);
                choiceContainer.SetActive(false);
                skipButton.SetActive(false);
                break;
        }
    }

    private void SetIssueText(string[] texts)
    {
        var issueContainerBehavior = issueContainer.GetComponent<IssueContainerBehavior>();
        if (issueContainerBehavior)
        {
            issueContainerBehavior.SetTexts(texts);
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

    private void SetRecapPlayerResponsesText(string choiceText1, string choiceText2, string choiceText3)
    {
        recapChoiceContainer.GetComponentInChildren<TextMeshProUGUI>().text =
            choiceText1 + " " + choiceText2 + " " + choiceText3 + " ";
    }
}
