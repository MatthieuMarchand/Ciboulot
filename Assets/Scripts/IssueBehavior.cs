using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IssueBehavior : MonoBehaviour
{
    [SerializeField] GameObject question1;
    [SerializeField] GameObject question2;
    bool question1Active = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePage();
        }
    }

    private void ChangePage()
    {
        if (question1Active)
        {
            question2.SetActive(true);
            question1.SetActive(false);
            question1Active = false;
        }
        else
        {
            question2.SetActive(false);
            question1.SetActive(true);
            question1Active = true;
        }

    }
}
