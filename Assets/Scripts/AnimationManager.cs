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
    private bool _rightAnim = true;
    private bool _goodSequence;
    
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

        if (bossAnimator == null || ciboulotAnimator == null)
        {
            bossAnimator = GameObject.FindWithTag("Boss").GetComponent<Animator>();
            ciboulotAnimator = GameObject.FindWithTag("Ciboulot").GetComponent<Animator>();

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
    }
    public void IntroAnimationOver()
    {
        bossAnimator.SetTrigger("boss_idle");
        introIsOver.Invoke();
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
    }
    private void BadAnimation()
    {
        bossAnimator.SetTrigger("boss_bad");
        ciboulotAnimator.SetTrigger("ciboulot_bad");
    }
    public void EndChoiceAnimationOver()
    {
        bossAnimator.SetTrigger("boss_idle");
        ciboulotAnimator.SetTrigger("ciboulot_idle");
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
            _goodSequence = true;
            HitAnimation();
        }
    }
    private void HitAnimation()
    {
        bossAnimator.SetTrigger(_rightAnim ? "boss_frappe_d" : "boss_frappe_g");
        //ciboulotAnimator.SetTrigger(_rightAnim ? "ciboulot_frappe_d" : "ciboulot_frappe_g");
        ciboulotAnimator.SetTrigger("ciboulot_frappe_d");
        _rightAnim = !_rightAnim;
    }
    private void SatisfyAnimation()
    {
        bossAnimator.SetTrigger("boss_satisfy");
    }
    public void EndSequenceAnimationOver()
    {
        bossAnimator.SetTrigger("boss_idle");
        ciboulotAnimator.SetTrigger("ciboulot_idle");
        gameManager.checkGameState.Invoke(_goodSequence);
    }
    
    //EndGame Animation
    private void PlayEndGameAnimation()
    {
        bossAnimator.SetTrigger("boss_happy");
        ciboulotAnimator.SetTrigger("ciboulot_happy");
    }
    public void OnEndGameAnimationOver()
    {
        gameManager.endScreen.Invoke();
    }

}
