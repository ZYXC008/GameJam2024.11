using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, TransitionIInteractable
{
    [Header("�¼�����")]
    public VoidEventSO SaveGameEvent;

    [Header("��������")]
    public bool isDone;

    private void OnEnable()
    {
        
    }
    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            //�ı䶯��

            SaveGameEvent.RaiseEvent();
            this.gameObject.tag = "Untagged";
        }
    }
}
