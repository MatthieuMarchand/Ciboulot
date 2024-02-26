using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;


public class SequenceManager : MonoBehaviour
{
    private float _timer;
    private bool _isTimerActivated;
    private bool _isIssueStep;
    
    public UnityEvent endSequence;
    public UnityEvent startChoiceStep;

    [FormerlySerializedAs("TimerUI")] [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject[] playerResponses;
    [SerializeField] private GameObject[] firstIssues;
    [SerializeField] private GameObject[] currentChoices;
    [FormerlySerializedAs("CurrentIssue")] [SerializeField] private GameObject currentIssue;
    [SerializeField] private GameObject issueContainer;
    [SerializeField] private GameObject choiceContainer;

    [SerializeField] private GameManager gameManager;

    //Getter and setter
    public float GetTimer()
    {
        return _timer;
    }

    public GameObject[] GetPlayerResponses()
    {
        return playerResponses;
    }
    //End Getter and setter
    
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        }
        if (issueContainer == null || timerUI == null || choiceContainer == null)
        {
            issueContainer = GameObject.FindGameObjectWithTag("IssueContainer");
            choiceContainer = GameObject.FindGameObjectWithTag("ChoiceContainer");
            timerUI = GameObject.FindGameObjectWithTag("Timer").gameObject;
        }
        gameManager.startNewSequence.AddListener(OnStartNewSequence);
        gameManager.startIntro.AddListener(OnStartGame);
        startChoiceStep.AddListener(StartChoice);

    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if (_isTimerActivated == false)
            return;
        
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            OnTimerOver();
        }
                
    }

    void OnStartGame()
    {
        choiceContainer.SetActive(false);
        timerUI.SetActive(false);
    }
    
    private void OnTimerOver()
    {
        _isTimerActivated = false;

        if (_isIssueStep)
        {
            _isIssueStep = false;
            startChoiceStep.Invoke();
        }
        else
        {
            SetChoiceToPlayerResponses(true);
        }
    }

    public void EndSequence()
    {
        endSequence.Invoke();
    }

    public void CheckResponses()
    {
        
    }
    
    public void CheckNumberOfResponses()
    {
        
    }

    public void OnStartNewSequence()
    {
        currentIssue = firstIssues[gameManager.GetSequenceNumber()];
        _timer = 5f;
        _isTimerActivated = true;
        _isIssueStep = true;
        SetUpUI(true);
    }

    private void SetUpUI(bool setIssue)
    {
        if (setIssue)
        {
            choiceContainer.SetActive(false);
            issueContainer.SetActive(true);
            timerUI.SetActive(true);
            SetIssueUI();
        }
        else
        {
            issueContainer.SetActive(false);
            choiceContainer.SetActive(true);
            SetChoiceUI();
            
        }
    }

    private void SetIssueUI()
    {
        for (int i = 0; i < issueContainer.transform.childCount; i++)
        {
            if (issueContainer.transform.GetChild(i).gameObject.name == "Question 1")
            {
                issueContainer.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    currentIssue.GetComponent<IssueBehavior>().GetissueText(1);
            }
            else if (issueContainer.transform.GetChild(i).gameObject.name == "Question 2")
            {
                issueContainer.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    currentIssue.GetComponent<IssueBehavior>().GetissueText(2);
            }
                
        }
    }
    private void SetChoiceUI()
    {
        choiceContainer.transform.Find("choix1").gameObject.transform.GetChild(0).gameObject
                .GetComponentInChildren<TextMeshProUGUI>().text =
            currentChoices[0].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
        choiceContainer.transform.Find("choix2").gameObject.transform.GetChild(0).gameObject
                .GetComponentInChildren<TextMeshProUGUI>().text =
            currentChoices[1].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
        choiceContainer.transform.Find("choix3").gameObject.transform.GetChild(0).gameObject
                .GetComponentInChildren<TextMeshProUGUI>().text =
            currentChoices[2].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
    }
    

    public void SetChoiceToPlayerResponses(bool isDefaultChoice)
    {
        _isTimerActivated = false;
        if (isDefaultChoice)
        {
            
        }
        else
        {
            
        }
    }
    
    
    private void StartChoice()
    {
        if (playerResponses.Length < 3)
        {
            currentChoices = currentIssue.GetComponent<IssueBehavior>().GetChoices();
            _timer = 5f;
            _isTimerActivated = true;
            _isIssueStep = true;
            SetUpUI(false);
        }
    }
}
