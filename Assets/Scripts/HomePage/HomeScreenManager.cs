using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace HomePage
{
    public class HomeScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject blackScreen;
        [SerializeField] private GameObject buttons;
        [SerializeField] private GameObject credits;

        private void Start()
        {
            if (!blackScreen)
            {
                Debug.LogError("blackscreen is not assigned");
                return;
            }
            blackScreen.GetComponent<Transition.Transition>().onAnimationOver.AddListener(() =>
            {
                SceneManager.LoadScene("GameScene");
            });
            blackScreen.SetActive(false);
        }

        public void StartGame()
        {
            if (!blackScreen)
            {
                Debug.LogError("blackscreen is not assigned");
                return;
            }
            blackScreen.SetActive(true);
            blackScreen.GetComponent<Transition.Transition>().FadeOut();
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            if (!blackScreen)
            {
                return;
            }
            blackScreen.GetComponent<Transition.Transition>().onAnimationOver.RemoveAllListeners();
        }

        public void ChangeCreditsVisibility(bool isVisible)
        {
            buttons.SetActive(!isVisible);
            credits.SetActive(isVisible);
        }
    }
}
