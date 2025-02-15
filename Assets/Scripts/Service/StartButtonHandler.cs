using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnButtonClicked()
    {
        if (!GameManager.Instance)
        {
            return;
        }
        if (!GameManager.Instance.GetIsGameStarted())
        {
            GameManager.Instance.StartGameHandler();
        }
        else
        {
            Debug.Log("hello");
            SequenceManager.Instance.OnTimerOver();
        }
    }
}
