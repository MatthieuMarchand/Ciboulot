using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBehavior : MonoBehaviour
{
    [SerializeField] private string ChoiceText;
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject defaultChoice;

    public string GetChoiceText()
    {
        return ChoiceText;
    }

    public GameObject[] GetChoices()
    {
        return choices;
    }
    
    public GameObject GetDefaultChoice()
    {
        return defaultChoice;
    }
}
