using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IssueContainerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject question1;
    [SerializeField] private GameObject question2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        if (question1.activeSelf == true)
        {
            question1.SetActive(false);
            question2.SetActive(true);
        }
        else
        {
            question1.SetActive(true);
            question2.SetActive(false);
        }
    }
}
