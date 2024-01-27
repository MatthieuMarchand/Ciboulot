using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    [SerializeField] public string text;
    [SerializeField] GameObject[] choices;

    public GameObject[] GetChoices()
    {
        return choices;
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
