using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, TransitionIInteractable
{
    public Animator anim;
    private AudioSource audioSource;
    [Header("事件监听")]
    public VoidEventSO SaveGameEvent;

    [Header("基本参数")]
    public bool isDone;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    public void TriggerAction()
    {
        isDone = true;
        //改变动画
        anim.SetBool("isDone", isDone);
        SaveGameEvent.RaiseEvent();
    }

    public void Confirm()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound/Confirm");
        audioSource.volume = 0.2f;
        audioSource.Play();
    }
}
