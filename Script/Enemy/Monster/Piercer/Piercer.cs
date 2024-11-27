using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Piercer : EnemyBase
{
    [Header("穿刺者特性")]
    public float detectionRadius = 5f; // 索敌范围
    public float chaseSpeed = 20f; // 追击速度
    public float standTimeAfterChase = 2f; // 追击后站立时间

    [HideInInspector] public Transform target; // 玩家目标

    public override void Awake()
    {
        base.Awake();

        // 初始化状态机
        patrolState = new PiercerStandState();
        chaseState = new PiercerChaseState();
        currentState = patrolState;

        // 初始化数据
        this.GetComponent<Attack>().damage = 8;
        character.maxHealth = 9;
        character.currentHealth = character.maxHealth;
    }

    private void Update()
    {
        if (!isDead)
        {
            PlayerInRangeCircle();
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

}