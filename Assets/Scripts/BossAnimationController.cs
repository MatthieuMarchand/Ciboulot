using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private AnimationManager animationManager;

    private void Awake()
    {
        if (animationManager == null)
        {
            animationManager = GameObject.FindWithTag("AnimationManager").gameObject.GetComponent<AnimationManager>();
        }
    }
    
    public void CallBackIntroAnimation()
    {
        Debug.Log("CallBackIntroAnimation");
        animationManager.IntroAnimationOver();
    }
    
    public void CallBackChoiceAnimation()
    {
        Debug.Log("CallBackGoodAnimation");
        animationManager.EndChoiceAnimationOver();
    }
    
    public void CallBackEndSequenceAnimation()
    {
        Debug.Log("CallBackEndSequenceAnimation");
        animationManager.EndSequenceAnimationOver();
    }
    
    public void CallBackEndGameAnimation()
    {
        Debug.Log("CallBackEndGameAnimation");
        animationManager.EndSequenceAnimationOver();
    }
}
