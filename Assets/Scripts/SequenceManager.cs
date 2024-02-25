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
    [FormerlySerializedAs("TimerUI")] [SerializeField] private GameObject timerUI;
    public UnityEvent endSequence;

    [SerializeField] private GameObject[] playerResponses;
    [SerializeField] private GameObject[] firstIssues;
    [SerializeField] private GameManager gameManager;
    [FormerlySerializedAs("CurrentIssue")] [SerializeField] private GameObject currentIssue;

    [SerializeField] private GameObject issueContainer;
    [SerializeField] private GameObject choiceContainer;

    //Getter and setter
    public float GetTimer()
    {
        return _timer;
    }
    
    
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
    
    private void OnTimerOver()
    {
        _isTimerActivated = false;

        if (_isIssueStep)
        {
            _isIssueStep = false;
            StartChoice();
        }
        else
        {
            _isIssueStep = true;
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
        issueContainer.SetActive(true);
        timerUI.SetActive(true);
        SetUpUI(true);
    }

    private void SetUpUI(bool setIssue)
    {
        if (setIssue)
        {
            issueContainer.SetActive(true);
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
        else
        {
            
        }
    }

    

    public void SetChoiceToPlayerResponses(bool isDefaultChoice)
    {
        
    }
    
    private void StartChoice()
    {
        if (playerResponses.Length < 3)
        {
            SetUpUI(false);
        }
    }
}
