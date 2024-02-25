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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CallBackIntroAnimation()
    {
        Debug.Log("CallBackIntroAnimation");
        animationManager.IntroAnimationOver();
    }
}
