using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    public Transform playerTransform;
    public PlayerController playerController;
    public Character character;
    private Animator anim;
    public GameObject signSprite;
    private IInteractable targetItem;
    private TransitionIInteractable target;
    public bool canPress;
    public bool canPick;
    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
        character = GetComponentInParent<Character>();
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }
    private void OnEnable()
    {
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }
    private void Update()
    {
        if (canPress || canPick) 
        { 
            signSprite.GetComponent<SpriteRenderer>().enabled = true ;
        }
        else
        {
            signSprite.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        signSprite.transform.localScale = playerTransform.localScale;
    }
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPick)
        {
            targetItem.TriggerAction(character);
        }
        if(canPress)
        {
            target.TriggerAction();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("Item") || other.CompareTag("Weapon") ))
        {
            if (playerController.isField) 
            {
                canPick = true;
                targetItem = other.GetComponent<IInteractable>();
            }
            else
            {
                canPick = false;
            }
        }
        if (other.CompareTag("IInteractable"))
        {
            canPress = true;
            target = other.GetComponent<TransitionIInteractable>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
        canPick = false;
    }
}
