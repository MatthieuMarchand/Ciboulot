using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    
    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        }
        
        gameManager.startGame.AddListener(IntroAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IntroAnimation()
    {
        Debug.Log("intro !");
    }
}
