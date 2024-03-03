using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace HomePage
{
    public class ButtonStartGameBehavior : MonoBehaviour
    {
        [SerializeField] private Button _buttonStartGame;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void LoadScene()
        {
            if (_buttonStartGame == null)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}
