using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, TransitionIInteractable
{
    public Animator anim;
    [Header("�¼�����")]
    public VoidEventSO SaveGameEvent;

    [Header("��������")]
    public bool isDone;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void TriggerAction()
    {
        isDone = true;
            //�ı䶯��
        anim.SetBool("isDone", isDone);
        SaveGameEvent.RaiseEvent();
    }
}
