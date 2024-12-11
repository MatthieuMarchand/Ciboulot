using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IssueBehavior : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private string[] issueTexts;
    [SerializeField] private GameObject defaultChoice;
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject[] goodChoices;

    public float GetTimer()
    {
        return timer;
    }
    
    //Setter and getter
    public string[] GetissueTexts()
    {
        return issueTexts;
    }

    public GameObject GetDefaultChoice()
    {
        return defaultChoice;
    }

    public GameObject[] GetChoices()
    {
        return choices;
    }
    public GameObject[] GetGoodChoices()
    {
        return goodChoices;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
