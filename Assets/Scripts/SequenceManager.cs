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
public class BoolEvent : UnityEvent<bool>
{
}
    
    private float _timer;
    private float timeAmount;
    private bool _isTimerActivated;
    private bool _isIssueStep;
    
    public UnityEvent endSequence;
    public UnityEvent startChoiceStep;
    [Tooltip("bool is 'isGoodAnimation'")]
    public BoolEvent BoolChoiceStep;
    [Tooltip("bool is 'isGoodAnimation'")]
    public BoolEvent BoolSequenceStep;
    

    [SerializeField] private GameObject[] playerResponses;
    [SerializeField] private GameObject[] firstIssues;
    [SerializeField] private GameObject[] currentChoices;
    [SerializeField] private GameObject[] goodChoices;

    [FormerlySerializedAs("CurrentIssue")] [SerializeField] private GameObject currentIssue;
    [FormerlySerializedAs("defaultChoice")] [SerializeField] private GameObject currentDefaultChoice;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;
    
    public static SequenceManager Instance { get; private set; }

    public GameObject GetCurrentChoice(int index)
    {
        return currentChoices[index];
    }
    //Getter and setter
    public float GetTimer()
    {
        return _timer;
    }
    
    public float GetTimeAmount()
    {
        return timeAmount;
    }

    public GameObject GetCurrentIssue()
    {
        return currentIssue;
    }

    public GameObject[] GetPlayerResponses()
    {
        return playerResponses;
    }
    
    public GameObject[] GetFirstIssues()
    {
        return firstIssues;
    }
    //End Getter and setter
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        }
        if (uiManager == null)
        {
            uiManager = FindAnyObjectByType<UIManager>().GetComponent<UIManager>();
        }
        gameManager.startNewSequence.AddListener(OnStartNewSequence);
        startChoiceStep.AddListener(StartChoice);
        BoolChoiceStep = new BoolEvent();
        BoolSequenceStep = new BoolEvent();

    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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
            startChoiceStep.Invoke();
        }
        else
        {
            SetChoiceToPlayerResponses(true);
        }
    }
    
    public void OnStartNewSequence()
    {
        currentIssue = firstIssues[gameManager.GetSequenceNumber()];
        _isTimerActivated = true;
        _isIssueStep = true;
        currentDefaultChoice = currentIssue.GetComponent<IssueBehavior>().GetDefaultChoice();
        _timer = currentIssue.GetComponent<IssueBehavior>().GetTimer();
        timeAmount = currentIssue.GetComponent<IssueBehavior>().GetTimer();
        playerResponses = Array.Empty<GameObject>();
        goodChoices = currentIssue.GetComponent<IssueBehavior>().GetGoodChoices();
        currentChoices = currentIssue.GetComponent<IssueBehavior>().GetChoices();
        
        uiManager.SetUpUI(UIManager.UIType.SetIssue, currentIssue.GetComponent<IssueBehavior>().GetissueTexts());
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

    public void ButtonSelected(int buttonNumber)
    {
        SetChoiceToPlayerResponses(false, currentChoices[buttonNumber]);
    }
    
    private void CheckNumberOfResponses()
    {
        if (playerResponses.Length < 3)
        {
            CheckLastPlayerChoice(playerResponses[^1]);
        }
        else
        {
            StartCoroutine(ShowResponsesAndWait());
        }
    }

    private IEnumerator ShowResponsesAndWait()
    {
        var recapResponses = new string[3];
        recapResponses[0] = playerResponses[0].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
        recapResponses[1] = playerResponses[1].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
        recapResponses[2] = playerResponses[2].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
        uiManager.SetUpUI(UIManager.UIType.SetRecapPlayersResponses, recapResponses);
        yield return new WaitForSeconds(5f);
        uiManager.SetUpUI(UIManager.UIType.HideRecap);
        CheckPlayerResponses();
    }
    
    private void CheckPlayerResponses()
    {
        var goodAnswers = 0;
        for (int i = 0; i < 3; i++)
        {
            if (goodChoices[i] == playerResponses[i])
            {
                goodAnswers++;
            }
        }
        BoolSequenceStep.Invoke(goodAnswers > 1);
    }
    
    private void CheckLastPlayerChoice(GameObject lastPlayerChoice)
    {
        uiManager.SetUpUI(UIManager.UIType.SetEndChoice);
        SetNextChoiceStep(lastPlayerChoice);
        BoolChoiceStep.Invoke(lastPlayerChoice == goodChoices[playerResponses.Length - 1]);
    }

    private void SetNextChoiceStep(GameObject lastPlayerChoice)
    {
        currentChoices = lastPlayerChoice.GetComponent<ChoiceBehavior>().GetChoices();
        currentDefaultChoice = lastPlayerChoice.GetComponent<ChoiceBehavior>().GetDefaultChoice();
    }
    
    private void StartChoice()
    {
        var currentChoicesText = new string[3];
        currentChoicesText[0] = currentChoices[0].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
        currentChoicesText[1] = currentChoices[1].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
        currentChoicesText[2] = currentChoices[2].gameObject.GetComponent<ChoiceBehavior>().GetChoiceText();
        _timer = 10f;
        timeAmount = 10f;
        _isTimerActivated = true;
        _isIssueStep = false;
        uiManager.SetUpUI(UIManager.UIType.SetChoice, currentChoicesText);
    }
}
