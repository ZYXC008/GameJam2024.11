using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool canPress;
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
        if (!playerController.isField)
        {
            canPress = false;
        }
    }
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if(canPress)
        {
            targetItem.TriggerAction(character);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("Item") || other.CompareTag("Weapon") )&& playerController.isField)
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
