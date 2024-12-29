using System;
using UnityEngine;

namespace Transition
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public void Start()
        {
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

        private void OnDestroy()
        {
            GameManager.Instance.startIntro.RemoveListener(FadeIn);
        }
    }
}
