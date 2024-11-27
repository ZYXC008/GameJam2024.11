using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFly : EnemyBase
{
    public Transform[] patrolPoints;  // 巡逻路径点
    private int currentPatrolIndex = 0; // 当前巡逻点
    public float detectionRadius = 5f; // 索敌范围
    private bool isPlayerInRange = false;
    [HideInInspector] public Transform target; // 获取玩家位置
    public override void Awake()
    {
        base.Awake();

        // 初始化状态机
        patrolState = new PiercerChaseState();
        chaseState = new PiercerChaseState();

        // 初始化数据
        this.GetComponent<Attack>().damage = 4;
        character.maxHealth = 9;
        character.currentHealth = character.maxHealth;
    }
    private void Start()
    {

    }

    private void Update()
    {
        if (!isDead)
        {
            currentState.LogicUpdate();
        }
    }

    void FixedUpdate()
    {
        if (!isDead && !isHurt)
        {
            currentState?.PhysicsUpdate();
        }
        Move();
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

    // 巡逻逻辑：沿着预设的巡逻点移动
    public override void Move()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex]; // 获得目标巡逻点位置
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.velocity = direction * normalSpeed;

        // 如果接近当前巡逻点，切换到下一个巡逻点
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // 先停止一秒然后切换到下一个巡逻点
            StartCoroutine(StayThenMove());
        }
    }

    IEnumerator StayThenMove()
    {
        StopMovement();
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        yield return new WaitForSeconds(1f);
    }

}