using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AnimationManager : MonoBehaviour
{
    
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SequenceManager sequenceManager;
    [FormerlySerializedAs("bossAnimation")] [SerializeField] private Animator bossAnimator;
    [FormerlySerializedAs("ciboulotAnimation")] [SerializeField] private Animator ciboulotAnimator;
    [SerializeField] private Animator feedbackAnimator;
    [SerializeField] private Animator rightHandAnimator;
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private Animator animatorBlackScreen;
    private bool _rightAnim = true;
    private bool _goodSequence;
    
    public UnityEvent introIsOver;

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

        if (bossAnimator == null || ciboulotAnimator == null || feedbackAnimator == null || animatorBlackScreen == null || rightHandAnimator == null || leftHandAnimator == null)
        {
            bossAnimator = GameObject.FindWithTag("Boss").GetComponent<Animator>();
            ciboulotAnimator = GameObject.FindWithTag("Ciboulot").GetComponent<Animator>();
            animatorBlackScreen = GameObject.FindWithTag("AnimationBlackScreen").GetComponent<Animator>();
            feedbackAnimator = GameObject.FindWithTag("Feedback").GetComponent<Animator>();
            rightHandAnimator = GameObject.FindWithTag("Hands").transform.Find("MainDroite").GetComponent<Animator>();
            leftHandAnimator = GameObject.FindWithTag("Hands").transform.Find("MainGauche").GetComponent<Animator>();
        }
        
        gameManager.startIntro.AddListener(IntroAnimation);
        gameManager.winGame.AddListener(PlayEndGameAnimation);

    }

    private void Start()
    {
        sequenceManager.BoolChoiceStep.AddListener(PlayEndChoiceAnimation);
        sequenceManager.BoolSequenceStep.AddListener(PlayEndSequenceAnimation);
    }

    //Intro Animation:
    private void IntroAnimation()
    {
        bossAnimator.SetTrigger("boss_intro");
        animatorBlackScreen.SetTrigger("fade_in");
    }
    
    //End Choice Animation:
    private void PlayEndChoiceAnimation(bool goodAnim)
    {
        if (goodAnim)
            GoodAnimation();
        else
            BadAnimation();
    }
    private void GoodAnimation()
    {
        bossAnimator.SetTrigger("boss_good");
        ciboulotAnimator.SetTrigger("ciboulot_good");
        feedbackAnimator.SetTrigger("feedback_GoodChoice");
    }
    private void BadAnimation()
    {
        bossAnimator.SetTrigger("boss_bad");
        ciboulotAnimator.SetTrigger("ciboulot_bad");
        feedbackAnimator.SetTrigger("feedback_BadChoice");
    }
    public void EndChoiceAnimationOver()
    {
        sequenceManager.startChoiceStep.Invoke();
    }
    
    //EndSequence Animation
    private void PlayEndSequenceAnimation(bool goodAnim)
    {
        if (goodAnim)
        {
            _goodSequence = true;
            SatisfyAnimation();
        }
        else
        {
            _goodSequence = false;
            HitAnimation();
        }
    }
    private void HitAnimation()
    {
        bossAnimator.SetTrigger(_rightAnim ? "boss_frappe_d" : "boss_frappe_g");
        ciboulotAnimator.SetTrigger(_rightAnim ? "ciboulot_frappe_d" : "ciboulot_frappe_g");

        if (_rightAnim)
            rightHandAnimator.SetTrigger("PlayRightHandAnimation");
        else
            leftHandAnimator.SetTrigger("PlayLeftHandAnimation");
        
        _rightAnim = !_rightAnim;
    }
    private void SatisfyAnimation()
    {
        bossAnimator.SetTrigger("boss_satisfy");
    }
    public void EndSequenceAnimationOver()
    {
        gameManager.checkGameState.Invoke(_goodSequence);
    }
    
    //EndGame Animation
    private void PlayEndGameAnimation()
    {
        ciboulotAnimator.SetTrigger("ciboulot_happy");
        bossAnimator.SetTrigger("boss_happy");
    }
    public void OnEndGameAnimationOver()
    {
        gameManager.endScreen.Invoke();
    }

}
