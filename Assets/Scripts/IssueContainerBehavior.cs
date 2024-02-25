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
        if (question1 || question2 == null)
        {
            question1 = transform.Find("Question 1").gameObject;
            question2 = transform.Find("Question 2").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        SwitchUI();
    }

    public void SwitchUI()
    {
        if (question1.activeSelf)
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
