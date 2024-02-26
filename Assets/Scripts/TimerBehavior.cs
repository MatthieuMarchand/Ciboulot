using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class TimerBehavior : MonoBehaviour
{
    [SerializeField] private SequenceManager _sequenceManager;

    [SerializeField] private Image UIAmount;
    [SerializeField] private TextMeshProUGUI UIText;

    
    // Start is called before the first frame update
    void Start()
    {
        if (_sequenceManager == null)
        {
            _sequenceManager = FindAnyObjectByType<SequenceManager>().GetComponent<SequenceManager>();
        }

        if (UIAmount == null || UIText == null)
        {
            UIAmount = transform.Find("FillTimer").GetComponent<Image>();
            UIText = transform.Find("tmp_timer").GetComponent<TextMeshProUGUI>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        UIAmount.fillAmount = _sequenceManager.GetTimer() / 5f;
        UIText.text = Mathf.Floor(_sequenceManager.GetTimer() + .9f).ToString();;
    }
}
