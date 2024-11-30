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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            Debug.Log("´¥·¢");
        }
    }
    public void OpenSwitch()
    {
        isDone = true;
        anim.SetTrigger("isDone");
    }
}
