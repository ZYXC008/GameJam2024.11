using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeast : EnemyBase
{
    [Header("黑之兽攻击状态设置")]
    public float attackTimer = 0;
    private float attackTime;
    public int changeStateCount = 0;
    private Transform player;       // 当前锁定的玩家
    private float distanceToPlayer; // 距离玩家的距离
    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").transform; // 根据标签锁定玩家
        // 设置初始生命值
        character.maxHealth = 90;
        character.currentHealth = 90;

        // 初始化状态
        patrolState = new BlackBeastPatrolState();
        chaseState = new BlackBeastStraightChargeState();
        attackState = new BlackBeastSideAttackState();
        jumpState = new BlackBeastJumpAttackState();

    }

    private void Update()
    {
        attackTime += Time.deltaTime;
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isDead)
        {
            // 看到玩家则切换攻击状态
            if (FoundPlayer())
            {
                ChangeState();
            }
            currentState?.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            currentState?.PhysicsUpdate();
            if (currentState == patrolState)
            {
                Move();
            }
        }



    }

    public override void Move()
    {
        if (!FoundPlayer())
        {
            // 基础巡逻移动逻辑
            base.Move();
        }

        else if (FoundPlayer())
        {
            // 计算敌人到玩家的方向
            Vector2 direction = (player.position - transform.position).normalized;

            // 获得朝向
            faceDir = new Vector2(Mathf.Sign(direction.x), faceDir.y);

            // 改变黑之兽的朝向
            transform.localScale = new Vector3(faceDir.x, 1, 1);

            // 使敌人朝玩家方向移动
            rb.velocity = direction * currentSpeed;
        }

    }

    public void ChangeState()
    {

        // 根据与玩家的距离决定攻击方式
        if (0 <= distanceToPlayer && distanceToPlayer <= 10f)
        {
            changeStateCount = 0;
        }

        if (50 <= distanceToPlayer && distanceToPlayer <= 100f)
        {
            changeStateCount = 1;
        }

        if (distanceToPlayer >= 100f)
        {
            changeStateCount = 2;
        }

        // 攻击cd结束后切换状态
        if (attackTime >= attackTimer)
        {
            attackTime = 0;
            switch (changeStateCount)
            {
                case 0:
                    this.SwichState(NPCState.Chase);
                    break;
                case 1:
                    this.SwichState(NPCState.Attack);
                    break;
                case 2:
                    this.SwichState(NPCState.Jump);
                    break;
                default:
                    break;
            }
        }

    }
}
