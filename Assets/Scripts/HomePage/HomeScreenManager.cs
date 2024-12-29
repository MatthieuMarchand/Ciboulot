using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace HomePage
{
    public class HomeScreenManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
