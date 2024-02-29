using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationManager : MonoBehaviour
{
    
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SequenceManager sequenceManager;
    [SerializeField] private Animator bossAnimation;
    [SerializeField] private Animator ciboulotAnimation;
    
    public UnityEvent introIsOver;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        }
        if (sequenceManager == null)
        {
            sequenceManager = FindAnyObjectByType<SequenceManager>().GetComponent<SequenceManager>();
        }

        if (bossAnimation == null || ciboulotAnimation == null)
        {
            bossAnimation = GameObject.FindWithTag("Boss").GetComponent<Animator>();
            ciboulotAnimation = GameObject.FindWithTag("Ciboulot").GetComponent<Animator>();

        }
        
        gameManager.startIntro.AddListener(IntroAnimation); 
    }

    private void Start()
    {
        sequenceManager.EndChoiceStep.AddListener(playEndChoiceAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IntroAnimation()
    {
        bossAnimation.SetTrigger("boss_intro");
    }

    public void IntroAnimationOver()
    {
        bossAnimation.SetTrigger("boss_idle");
        introIsOver.Invoke();
    }

    private void playEndChoiceAnimation(bool goodAnim)
    {
        if (goodAnim)
            GoodAnimation();
        else
            BadAnimation();
    }
    private void GoodAnimation()
    {
        bossAnimation.SetTrigger("boss_good");
    }

    public void EndChoiceAnimationOver()
    {
        bossAnimation.SetTrigger("boss_idle");
        sequenceManager.startChoiceStep.Invoke();
    }
    
    private void BadAnimation()
    {
        bossAnimation.SetTrigger("boss_bad");
    }
}
