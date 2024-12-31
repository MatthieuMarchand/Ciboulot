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
    [FormerlySerializedAs("Issues")] [FormerlySerializedAs("firstIssues")] [SerializeField] private GameObject[] issues;
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
    
    public GameObject[] GetIssues()
    {
        return issues;
    }
    //End Getter and setter
    
    /**
     * Returns a Dictionary of text for key and isBoss for value
     */
    public Dictionary<string, bool> GetAllTexts()
    {
        var outTexts = new Dictionary<string, bool>();
        var issueContainer = GameObject.FindGameObjectWithTag("IssueContainer").GetComponent<IssueContainerBehavior>();
        //Intro
        if (issueContainer)
        {
            foreach (var text in issueContainer.GetTexts())
            {
                outTexts.TryAdd(text, true);
            }
        }
        //Issues
        foreach (var issue in issues)
        {
            if (!issue)
            {
                continue;
            }

            IssueBehavior issueBehavior = issue.GetComponent<IssueBehavior>();
            if (!issueBehavior)
            {
                continue;
            }
            foreach (var text in issueBehavior.GetissueTexts())
            {
                outTexts.TryAdd(text, true);
            }
            //Choices
            foreach (var choice in issueBehavior.GetChoices())
            {
                if (!choice)
                {
                    continue;
                }

                ChoiceBehavior choiceBehavior = choice.GetComponent<ChoiceBehavior>();
                if (!choiceBehavior)
                {
                    continue;
                }

                List<string> choiceTexts = new List<string>(); 
                GetChildrenChoiceTexts(choice, choiceTexts);
                foreach (var choiceText in choiceTexts)
                {
                    outTexts.TryAdd(choiceText, false);
                }
            }
        }

        return outTexts;
    }

    //Get all the texts of children issues
    private void GetChildrenChoiceTexts(GameObject choice, List<string> texts)
    {
        if (!choice)
        {
            return;
        }
        ChoiceBehavior choiceBehavior = choice.GetComponent<ChoiceBehavior>();
        if (!choiceBehavior)
        {
            return;   
        }

        texts.Add(choiceBehavior.GetChoiceText());
        if (choiceBehavior.GetChoices().Length < 1)
        {
            return;
        }

        foreach (var childChoice in choiceBehavior.GetChoices())
        {
            GetChildrenChoiceTexts(childChoice, texts);
        }
    }
    
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
        currentIssue = issues[gameManager.GetSequenceNumber()];
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
