using System;
using UnityEngine;
using UnityEngine.Events;

namespace Transition
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public UnityEvent onAnimationOver;
        public void Start()
        {
            if (!GameManager.Instance)
            {
                return;
            }
            GameManager.Instance.startIntro.AddListener(FadeIn);
        }

        public void SetBlackTransitionToFalse()
        {
            gameObject.SetActive(false);
        }

        private void FadeIn()
        {
            animator.SetTrigger("fade_in");
        }
        
        public void FadeOut()
        {
            animator.SetTrigger("fade_out");
        }

        public void OnFadeOutOver()
        {
            onAnimationOver.Invoke();
        }

        private void OnDestroy()
        {
            if (!GameManager.Instance)
            {
                return;
            }
            GameManager.Instance.startIntro.RemoveListener(FadeIn);
        }
    }
}
