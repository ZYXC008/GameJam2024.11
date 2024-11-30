using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCorpseElite : EnemyBase
{
    [Header("狂梦尸特性")]
    public float detectionRadius = 5f; // 索敌范围
    public float attackRadius = 3f;  // 攻击范围
    public Transform attackPos; // 攻击位置

    [HideInInspector] public Transform target; // 玩家目标

    public override void Awake()
    {
        base.Awake();
        character.maxHealth = 30;
        character.currentHealth = character.maxHealth;
        this.GetComponent<Attack>().damage = 8;
        attackPos.gameObject.GetComponent<Attack>().damage = 0;

        // 初始化状态机
        patrolState = new DreamCorpseElitePatrolState();
        attackState = new DreamCorpseEliteAttackState();
        currentState = patrolState;
        currentState.OnEnter(this);
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


    private void OnDrawGizmos()
    {
        // 可视化攻击范围
        if (attackPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRadius);
        }

        // 可视化索敌范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void AttackPlayer()
    {
        Debug.Log("执行狂梦尸攻击");
        // 获取攻击范围内的玩家对象
        Collider2D hit = Physics2D.OverlapCircle(attackPos.position, attackRadius, attackLayer);
        if (hit == null)
        {
            Debug.Log("没有找到玩家对象");
        }
        if (hit != null && hit.TryGetComponent(out Character character))
        {
            // 造成伤害
            attackPos.gameObject.GetComponent<Attack>().damage = 8;
            character.TakeDamage(attackPos.gameObject.GetComponent<Attack>());
            Debug.Log($"Hit {character.gameObject.name} for {attackPos.gameObject.GetComponent<Attack>().damage} damage.");
        }

    }

    public void ResetAttackDamage()
    {
        attackPos.gameObject.GetComponent<Attack>().damage = 0;
    }
}
