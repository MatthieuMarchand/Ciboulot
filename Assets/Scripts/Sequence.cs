using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sequence : MonoBehaviour
{
    [SerializeField] string issue;

    [SerializeField] GameObject[] firstChoices;
    [SerializeField] GameObject[] validChoices;


    //Setter et Getter

    
    public GameObject[] GetValidChoices()
    {
        return validChoices;
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
