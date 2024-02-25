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
    private bool isTimerActivated;
    [SerializeField] private GameObject TimerUI;
    public UnityEvent endSequence;

    [SerializeField] private GameObject[] playerResponses;
    [SerializeField] private GameObject[] firstIssues;
    [SerializeField] private GameManager gameManager;
    [FormerlySerializedAs("CurrentIssue")] [SerializeField] private GameObject currentIssue;

    [SerializeField] private GameObject issueContainer;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        }
        if (issueContainer == null)
        {
            issueContainer = GameObject.FindGameObjectWithTag("IssueContainer");
        }
        gameManager.startNewSequence.AddListener(OnStartNewSequence);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (isTimerActivated == false)
            return;
        
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            //todo
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
        isTimerActivated = true;
        SetUpUI();
    }

    void SetUpUI()
    {
        if (playerResponses.Length < 3)
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
    }
}
