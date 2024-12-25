using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IssueContainerBehavior : MonoBehaviour
{
    [FormerlySerializedAs("question1")] [SerializeField] private GameObject question;
    [FormerlySerializedAs("question2")] [SerializeField] private GameObject textBox;
    [SerializeField] private string[] texts;
    private int _currentIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (question == null || textBox == null)
        {
            question = transform.Find("background_question").gameObject;
            textBox = question.transform.Find("text_question").gameObject;
        }
        SetCurrentText();
    }

    public void SetTexts(string[] newTexts)
    {
        texts = newTexts;
        _currentIndex = 0;
        SetCurrentText();
    }

    private void SetCurrentText()
    {
        TextMeshProUGUI text = textBox.GetComponent<TextMeshProUGUI>();
        if (text)
        {
            text.SetText(texts[_currentIndex]);
            SoundManager.Instance.PlayAnimaleseDialogueFromText(texts[_currentIndex]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        SwitchUI(true);
    }

    public void SwitchUI(bool isNext)
    {
        if (texts.Length == 0) return;
        _currentIndex = isNext ? _currentIndex + 1 : _currentIndex - 1;
        if (_currentIndex >= texts.Length)
        {
            _currentIndex = 0;
        }
        else if (_currentIndex < 0)
        {
            _currentIndex = texts.Length - 1;
        }

        SetCurrentText();
    }
}
