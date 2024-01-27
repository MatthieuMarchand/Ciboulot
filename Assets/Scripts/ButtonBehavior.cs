using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] GameObject choice;
    [SerializeField] SequenceManager sequenceManager;

    // Start is called before the first frame update
    void Start()
    {
        if (sequenceManager == null)
        {
            sequenceManager = GameObject.FindGameObjectWithTag("SequenceManager").GetComponent<SequenceManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChoiceSelected()
    {
        sequenceManager.AddChoiceToPlayerResponses(choice);
        sequenceManager.CheckIfResponseIsComplete(choice);
    }

    public void SetChoice(GameObject newChoice)
    {
        choice = newChoice;
        Choice choiceBehavior = choice.GetComponent<Choice>();
        gameObject.GetComponentInChildren<TMP_Text>().text = choiceBehavior.text;
    }
}
