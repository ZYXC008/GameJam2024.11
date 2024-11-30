using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Piercer : EnemyBase
{
    [Header("穿刺者特性")]
    public float detectionRadius = 5f; // 索敌范围
    public float standTimeAfterChase = 2f; // 追击后站立时间

    [HideInInspector] public Transform target; // 玩家目标
    private AudioSource audioSource;
    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        // 初始化状态机
        patrolState = new PiercerStandState();
        chaseState = new PiercerChaseState();
        currentState = patrolState;
        currentState.OnEnter(this);

        // 初始化数据
        attack.damage = 8;
        character.maxHealth = 9;
        character.currentHealth = character.maxHealth;
    }

    private void Update()
    {
        // 设置朝向（根据对象的X轴缩放值决定）
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if (!isDead)
        {
            currentState?.LogicUpdate();
        }
    }

    void FixedUpdate()
    {
        if (!isDead && !isHurt)
        {
            currentState?.PhysicsUpdate();
        }

    }
    public override void Move()
    {
        // 不需要实现一般移动逻辑，追击逻辑由 ChaseState 执行
    }

    public bool PlayerInRangeCircle()
    {
        // 检测玩家是否在索敌范围内
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, attackLayer);
        if (hit != null)
        {
            target = hit.transform;
            return true;
        }
        target = null;
        return false;
    }



    private void OnDrawGizmos()
    {
        // 可视化索敌范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void AttackSound()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound/SideAttack");
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}