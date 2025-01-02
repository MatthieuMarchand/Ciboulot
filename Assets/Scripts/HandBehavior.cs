using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class HandBehavior : MonoBehaviour
{
    [SerializeField] private Sprite[] hands;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    // Start is called before the first frame update
    void Awake()
    {
        if (leftHand && rightHand)
            return;
        rightHand = transform.Find("MainDroite").gameObject;
        rightHand = transform.Find("MainGauche").gameObject;
    }

    private void Start()
    {
        GameManager.Instance.lifeRemoved.AddListener(ChangeHandsTexture);
    }

    private void ChangeHandsTexture()
    {
        int lifeNumber = GameManager.Instance.GetLives();
        switch (lifeNumber)
        {
            case 1:
                // rightHand.GetComponent<Image>().sprite = hands[5];
                leftHand.GetComponent<Image>().sprite = hands[2];
                break;
            case 2:
                rightHand.GetComponent<Image>().sprite = hands[5];
                // leftHand.GetComponent<Image>().sprite = hands[2];
                break;
            case 3:
                rightHand.GetComponent<Image>().sprite = hands[3];
                leftHand.GetComponent<Image>().sprite = hands[0];
                break;
                
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.lifeRemoved.RemoveListener(ChangeHandsTexture);
    }
}
