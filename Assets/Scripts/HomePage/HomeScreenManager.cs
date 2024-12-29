using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace HomePage
{
    public class HomeScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject blackScreen;

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
            blackScreen.GetComponent<Transition.Transition>().onAnimationOver.RemoveAllListeners();
        }
    }
}
