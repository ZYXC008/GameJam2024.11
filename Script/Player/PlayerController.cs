using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Rigidbody2D rb;
    public FieldController fieldMask;
    public Vector2 inputDirection;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D coll;
    private Character character;
    private PlayerWeaponController weaponController;
    [Header("事件监听")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    [Header("基本参数")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    public int jumpTimes;
    public int currentJumpTimes;
    public float sprintForce;
    [Header("计时器")]
    public float sprintTime;
    public float sprintTimeCounter;
    
    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool isField;
    public bool isSprint;
    public bool isUseSkill;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        character = GetComponent<Character>();
        
        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.Attack.started += PlayerAttack;
        inputControl.Gameplay.Field.started += Field;
        inputControl.Gameplay.Sprint.started += IsSprint;
        inputControl.Gameplay.UseSkill.started += UseSkill;
    }

    private void OnEnable()
    {
        inputControl.Enable();
        //制作场景切换时解除注释
        //loadEvent.LoadRequestEvent += OnLoadEvent;
        //afterSceneLoadedEvent.OnEventRiased += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        //loadEvent.LoadRequestEvent -= OnLoadEvent;
        //afterSceneLoadedEvent.OnEventRiased -= OnAfterSceneLoadedEvent;
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isDead /*&& !isAttack*/)
        {
            Move();
            Sprint();
        }
            
        character.FieldOpen(isField);
    }

    public void Move()//移动
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        int faceDir = (int)transform.localScale.x;
        
        if (inputDirection.x < 0)
            faceDir = -1;
        if (inputDirection.x > 0)
            faceDir = 1;
        transform.localScale = new Vector3(faceDir, 1, 1);
        
    }
    public void Sprint()
    {
        if (isSprint)
        {
            if(sprintTimeCounter <= sprintTime)
            {
                rb.AddForce(inputDirection * sprintForce, ForceMode2D.Impulse);
                sprintTimeCounter += Time.fixedDeltaTime;
            }
            else
            {
                isSprint = false;
                sprintTimeCounter = 0;
            }
        }
        
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround || (!physicsCheck.isGround && currentJumpTimes != jumpTimes))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            currentJumpTimes++;
        }
    }//跳跃
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        //playerAnimation.PlayAttack();
        isAttack = true;
        weaponController = GetComponent<PlayerWeaponController>();
        if (weaponController.buffTimes > 0)
        {
            weaponController.buffTimes--;
        }
        
    }    //攻击

    private void UseSkill(InputAction.CallbackContext context)
    {
        weaponController = GetComponent<PlayerWeaponController>();
        isUseSkill = true;
        weaponController.skillAttack.Skill();
    }
    private void Field(InputAction.CallbackContext context)
    {
        isField = !isField;
        fieldMask.SetField(isField);
    }//领域展开
    private void IsSprint(InputAction.CallbackContext context)
    {
        isSprint = true;
        character.invulnerable = true;
        character.invulnerableCounter = sprintTime;
    }//冲刺
    //场景加载禁止移动
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }
    //场景加载完允许移动
    private void OnAfterSceneLoadedEvent()
    {
        inputControl.Gameplay.Enable();
    }
    public void GetHurt(Transform attcker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attcker.transform.position.x, 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    public void CheckState()
    {
        if (physicsCheck.isGround && currentJumpTimes != 0)
        {
            currentJumpTimes = 0;
        }
    }
    
   
}
