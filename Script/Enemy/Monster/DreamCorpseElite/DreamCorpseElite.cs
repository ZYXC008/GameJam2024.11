using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCorpseElite : EnemyBase
{
    [Header("狂梦尸特性")]
    public float detectionRadius = 5f; // 索敌范围

    [HideInInspector] public Transform target; // 玩家目标

    public override void Awake()
    {
        base.Awake();
        character.maxHealth = 30;
        character.currentHealth = character.maxHealth;
        this.GetComponent<Attack>().damage = 8;
        patrolState = new DreamCorpseElitePatrolState();
        attackState = new DreamCorpseEliteAttackState();
    }

    private void Start()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GetHurt(transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // 可视化索敌范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
