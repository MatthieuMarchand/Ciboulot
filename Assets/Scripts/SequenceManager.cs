using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
[Tooltip("bool is 'isGoodAnimation'")]

public class SequenceManager : MonoBehaviour
{
public class EndChoiceEvent : UnityEvent<bool>
{
}
    private float _timer;
    private bool _isTimerActivated;
    private bool _isIssueStep;
    
    public UnityEvent endSequence;
    public UnityEvent startChoiceStep;
    public EndChoiceEvent endChoiceStep;
    

    [SerializeField] private GameObject[] playerResponses;
    [SerializeField] private GameObject[] firstIssues;
    [SerializeField] private GameObject[] currentChoices;
    [SerializeField] private GameObject[] goodChoices;

    [FormerlySerializedAs("CurrentIssue")] [SerializeField] private GameObject currentIssue;
    [SerializeField] private GameObject defaultChoice;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;

    

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
        if (uiManager == null)
        {
            uiManager = FindAnyObjectByType<UIManager>().GetComponent<UIManager>();
        }
        gameManager.startNewSequence.AddListener(OnStartNewSequence);
        gameManager.startIntro.AddListener(OnStartGame);
        startChoiceStep.AddListener(StartChoice);
        endChoiceStep = new EndChoiceEvent();

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
        uiManager.SetUpUI(UIManager.UIType.SetStartGame);
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
    
    public void OnStartNewSequence()
    {
        currentIssue = firstIssues[gameManager.GetSequenceNumber()];
        _timer = 5f;
        _isTimerActivated = true;
        _isIssueStep = true;
        defaultChoice = currentIssue.GetComponent<IssueBehavior>().GetDefaultChoice();
        playerResponses = new GameObject[0];
        goodChoices = currentIssue.GetComponent<IssueBehavior>().GetGoodChoices();
        
        
        uiManager.SetUpUI(UIManager.UIType.SetIssue, currentIssue.GetComponent<IssueBehavior>().GetissueText(1), currentIssue.GetComponent<IssueBehavior>().GetissueText(2));
    }
    public void SetChoiceToPlayerResponses(bool isDefaultChoice, GameObject choiceSelected = null)
    {
        _isTimerActivated = false;
        if (isDefaultChoice)
        {
            List<GameObject> listeResponses = new List<GameObject>(playerResponses);
            listeResponses.Add(defaultChoice);
            playerResponses = listeResponses.ToArray();
        }
        else
        {
            List<GameObject> listeResponses = new List<GameObject>(playerResponses);
            listeResponses.Add(choiceSelected);
            playerResponses = listeResponses.ToArray();
        }
        CheckNumberOfResponses();
    }

    public void ButtonSelected(GameObject choiceSelected)
    {
        SetChoiceToPlayerResponses(choiceSelected);
    }
    
    private void CheckNumberOfResponses()
    {
        
        if (playerResponses.Length < 3)
        {
            CheckLastPlayerChoice(playerResponses[^1]);
        }
        else if (playerResponses.Length == 3)
        {
            //TODO EndSequence
        }
    }
    private void CheckLastPlayerChoice(GameObject lastPlayerChoice)
    {
        uiManager.SetUpUI(UIManager.UIType.SetEndChoice);
        if (lastPlayerChoice == goodChoices[playerResponses.Length -1])
        {
            endChoiceStep.Invoke(true);
        }
        else
        {
            endChoiceStep.Invoke(false);
        }
    }
    private void StartChoice()
    {
        if (playerResponses.Length < 3)
        {
            currentChoices = currentIssue.GetComponent<IssueBehavior>().GetChoices();
            _timer = 5f;
            _isTimerActivated = true;
            _isIssueStep = false;
            uiManager.SetUpUI(UIManager.UIType.SetChoice, currentChoices[0].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText(), currentChoices[1].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText(), currentChoices[2].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText());
        }
    }
}
