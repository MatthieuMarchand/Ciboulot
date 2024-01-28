using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int fingers;
    [SerializeField] int score;
    const int WIN_SCORE = 5;
    [SerializeField] SequenceManager sequenceManager;
    [SerializeField] public GameObject boss;
    [SerializeField] public Animator bossAnimator;
    [SerializeField] public GameObject timer;
    [SerializeField] public GameObject choiceContainer;
    [SerializeField] public GameObject issueContainer;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (sequenceManager == null)
        {
            sequenceManager = GameObject.FindGameObjectWithTag("SequenceManager").GetComponent<SequenceManager>();
        }
        if (boss == null || bossAnimator == null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            bossAnimator = boss.GetComponent<Animator>();
        }
        if (timer == null)
        {
            timer = GameObject.FindGameObjectWithTag("Timer");
        }
        StartCoroutine(Init());


    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public void WinSequence()
    {
        score++;
        if (score == WIN_SCORE) 
        {
            StartCoroutine(Happy());
        }
        //Appelle WinScript de fin
        else 
        {
            StartCoroutine(Satisfy());
        }
    }

    public void LoseSequence()
    {
        fingers--;
        if (fingers == 0) 
        {
            StartCoroutine(Angry());
            //loseScript
        }
        else
            StartCoroutine(Frappe());
    }

    public void StartSequence()
    {
        sequenceManager.SetCurrentSequence(score);
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        //choiceContainer = GameObject.FindGameObjectWithTag("ChoiceContainer");
        choiceContainer.SetActive(false);
        //issueContainer = GameObject.FindGameObjectWithTag("IssueContainer");
        issueContainer.SetActive(true);

        float animationTime = 2f;
        timer.GetComponent<TimerBehavior>().thinkTime = animationTime;
        sequenceManager.SetTimer(animationTime);
        yield return new WaitForSeconds(animationTime);

        //Logique
        choiceContainer.SetActive(true);
        issueContainer.SetActive(false);
        sequenceManager.SetIssueText();
        sequenceManager.SetButtonsChoices(sequenceManager.GetCurrentSequence().GetComponent<Sequence>().GetFirstChoices());

    }

    IEnumerator Satisfy() //quand tu gagnes une s�quence
    {
        boss.GetComponent<Animator>().SetBool("boss_satisfy", true);
        yield return new WaitForSeconds(2.14f);
        boss.GetComponent<Animator>().SetBool("boss_satisfy", false);

        StartSequence();

    }

    IEnumerator Frappe()
    {
        boss.GetComponent<Animator>().SetBool("boss_frappe", true);
        yield return new WaitForSeconds(2.22f);
        boss.GetComponent<Animator>().SetBool("boss_frappe", false);
        StartSequence();

    }
    IEnumerator Happy()
    {
        boss.GetComponent<Animator>().SetBool("boss_happy", true);
        yield return new WaitForSeconds(6.0f);
        boss.GetComponent<Animator>().SetBool("boss_happy", false);
    }

    IEnumerator Angry()
    {
        boss.GetComponent<Animator>().SetBool("boss_angry", true);
        yield return new WaitForSeconds(1.02f);
        boss.GetComponent<Animator>().SetBool("boss_angry", false);
    }
}
