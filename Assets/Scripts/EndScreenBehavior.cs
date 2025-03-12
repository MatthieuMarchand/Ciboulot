using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenBehavior : MonoBehaviour
{
    public static void LoadMenuScene()
    {
        SceneManager.LoadScene("HomeScreen");
    }
    
    public static void QuitGame()
    {
        Application.Quit();
    }
}
