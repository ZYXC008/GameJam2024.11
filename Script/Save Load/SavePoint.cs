using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, TransitionIInteractable
{
    [Header("事件监听")]
    public VoidEventSO SaveGameEvent;

    [Header("基本参数")]
    public bool isDone;

    private void OnEnable()
    {
        
    }
    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            //改变动画

            SaveGameEvent.RaiseEvent();
            this.gameObject.tag = "Untagged";
        }
    }
}
