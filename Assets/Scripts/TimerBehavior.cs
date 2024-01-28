using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBehavior : MonoBehaviour
{
    [SerializeField] SequenceManager sequenceManager;
    float fillAmount;
    [SerializeField] Image timerImage;
    public float thinkTime;
    public float GetThinkTime()
    {
        return thinkTime;
    }
    
    public void SetThinkTime(float newThinkTime)
    {
        thinkTime = newThinkTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (sequenceManager == null)
        {
            sequenceManager = GameObject.FindGameObjectWithTag("SequenceManager").GetComponent<SequenceManager>();
        }
        //fillAmount = GetComponentInChildren<Image>().fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        timerImage.fillAmount = sequenceManager.GetTimer()/thinkTime;
    }
}
