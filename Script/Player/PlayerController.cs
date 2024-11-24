using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Rigidbody2D rb;
    public Vector2 inputDirection;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D coll;
    [Header("�¼�����")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    [Header("��������")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    public int jumpTimes;
    public int currentJumpTimes;
    [Header("״̬")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }



    private void OnEnable()
    {
        inputControl.Enable();
        //���������л�ʱ���ע��
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
        Move();
    }

    public void Move()//�ƶ�
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x < 0)
            faceDir = -1;
        if (inputDirection.x > 0)
            faceDir = 1;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround || (!physicsCheck.isGround && currentJumpTimes != jumpTimes))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            currentJumpTimes++;
        }
    }//��Ծ

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        //playerAnimation.PlayAttack();
        isAttack = true;
    }    //����

    //�������ؽ�ֹ�ƶ�
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }
    //���������������ƶ�
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
