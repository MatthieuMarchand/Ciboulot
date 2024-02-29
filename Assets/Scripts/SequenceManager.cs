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
public class EndEvent : UnityEvent<bool>
{
}
    
    private float _timer;
    private bool _isTimerActivated;
    private bool _isIssueStep;
    
    public UnityEvent endSequence;
    public UnityEvent startChoiceStep;
    public EndEvent EndChoiceStep;
    public EndEvent EndSequenceStep;
    

    [SerializeField] private GameObject[] playerResponses;
    [SerializeField] private GameObject[] firstIssues;
    [SerializeField] private GameObject[] currentChoices;
    [SerializeField] private GameObject[] goodChoices;

    [FormerlySerializedAs("CurrentIssue")] [SerializeField] private GameObject currentIssue;
    [FormerlySerializedAs("defaultChoice")] [SerializeField] private GameObject currentDefaultChoice;
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
        EndChoiceStep = new EndEvent();

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
        currentDefaultChoice = currentIssue.GetComponent<IssueBehavior>().GetDefaultChoice();
        playerResponses = new GameObject[0];
        goodChoices = currentIssue.GetComponent<IssueBehavior>().GetGoodChoices();
        
        
        uiManager.SetUpUI(UIManager.UIType.SetIssue, currentIssue.GetComponent<IssueBehavior>().GetissueText(1), currentIssue.GetComponent<IssueBehavior>().GetissueText(2));
    }
    private void SetChoiceToPlayerResponses(bool isDefaultChoice, GameObject choiceSelected = null)
    {
        _isTimerActivated = false;
        if (isDefaultChoice)
        {
            List<GameObject> listeResponses = new List<GameObject>(playerResponses);
            listeResponses.Add(currentDefaultChoice);
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
            CheckPlayerResponses();
        }
    }
    
    private void CheckPlayerResponses()
    {
        int goodAnswers = 0;
        for (int i = 0; i < 3; i++)
        {
            if (goodChoices[i] == playerResponses[i])
            {
                goodAnswers++;
            }
        }
        
        if (goodAnswers > 2)
        {
            Debug.Log("Good !");
            //Todo Play good end sequence animation
        }
        else
        {
            Debug.Log("Bad !");
            //todo play bad sequence animation
        }
    }
    
    private void CheckLastPlayerChoice(GameObject lastPlayerChoice)
    {
        uiManager.SetUpUI(UIManager.UIType.SetEndChoice);
        if (lastPlayerChoice == goodChoices[playerResponses.Length -1])
        {
            
            EndChoiceStep.Invoke(true);
        }
        else
        {
            EndChoiceStep.Invoke(false);
        }
    }

    private void SetNextChoiceStep(GameObject lastPlayerChoice)
    {
        currentChoices = lastPlayerChoice.GetComponent<ChoiceBehavior>().GetChoices();
        currentDefaultChoice = lastPlayerChoice.GetComponent<ChoiceBehavior>().GetDefaultChoice();
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
