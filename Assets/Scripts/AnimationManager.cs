using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationManager : MonoBehaviour
{
    
    [SerializeField] private GameManager gameManager;
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

        if (bossAnimation || ciboulotAnimation == null)
        {
            bossAnimation = GameObject.FindWithTag("Boss").GetComponent<Animator>();
            ciboulotAnimation = GameObject.FindWithTag("Ciboulot").GetComponent<Animator>();

        }
        
        gameManager.startIntro.AddListener(IntroAnimation);
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
}
