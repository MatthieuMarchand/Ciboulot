using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] GameObject choice;
    [SerializeField] SequenceManager sequenceManager;

    public void SetChoice(GameObject newChoice)
    {
        choice = newChoice;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sequenceManager == null)
        {
            sequenceManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SequenceManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChoiceSelected()
    {
        sequenceManager.validSequence(choice);
    }
}
