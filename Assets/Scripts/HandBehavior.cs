using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class HandBehavior : MonoBehaviour
{
    [SerializeField] private Sprite[] hands;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager && leftHand && rightHand)
            return;
        gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        rightHand = transform.Find("MainDroite").gameObject;
        rightHand = transform.Find("MainGauche").gameObject;
    }

    private void Start()
    {
        gameManager.lifeRemoved.AddListener(ChangeHandsTexture);
    }

    private void ChangeHandsTexture()
    {
        int lifeNumber = gameManager.GetLives();
        switch (lifeNumber)
        {
            case 1:
                rightHand.GetComponent<Image>().sprite = hands[5];
                leftHand.GetComponent<Image>().sprite = hands[2];
                break;
            case 2:
                rightHand.GetComponent<Image>().sprite = hands[4];
                leftHand.GetComponent<Image>().sprite = hands[1];
                break;
            case 3:
                rightHand.GetComponent<Image>().sprite = hands[3];
                leftHand.GetComponent<Image>().sprite = hands[0];
                break;
                
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
