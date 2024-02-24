using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IssueBehavior : MonoBehaviour
{
    [SerializeField] private string issueText1;
    [SerializeField] private string issueText2;
    [SerializeField] private GameObject defaultChoice;
    [SerializeField] private GameObject[] choices;

    //Setter and getter
    public string GetissueText(int part)
    {
        string textToReturn = "";
        if (part == 1)
            textToReturn = issueText1;
        if (part == 2)
            textToReturn = issueText2;

        return textToReturn;
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
