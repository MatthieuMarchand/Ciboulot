using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChoiceBehavior : MonoBehaviour
{
    public void OnButtonEnter(int index)
    {
        if (!SequenceManager.Instance && SequenceManager.Instance.GetCurrentChoice(index))
        {
            return;
        }
        
        SoundManager.Instance.PlaySound(SequenceManager.Instance.GetCurrentChoice(index).GetComponent<ChoiceBehavior>().GetAudioClip());
    }
}
