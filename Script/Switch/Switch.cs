using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, TransitionIInteractable
{
    private SpriteRenderer spriteRenderer;
    public bool isDone;
    public Sprite openSprite;
    public Sprite closeSprite;
    public Animator anim;
    public AudioSource audioSource;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }
    public void TriggerAction()
    {
        if (!isDone)
        {
            OpenSwitch();
            Debug.Log("触发");
        }
    }
    public void OpenSwitch()
    {
        isDone = true;
        anim.SetTrigger("isDone");
        audioSource.clip = Resources.Load<AudioClip>("Sound/SwitchOpen");
        audioSource.volume = 0.2f;
        audioSource.Play();
    }
}
