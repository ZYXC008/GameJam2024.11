using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCorpseElite : EnemyBase
{
    [Header("狂梦尸特性")]
    public float detectionRadius = 5f; // 索敌范围
    public float attackRadius = 3f;  // 攻击范围
    public Transform attackPos; // 攻击位置
    [HideInInspector] public Transform target; // 攻击目标
    [HideInInspector] public Transform player; // 玩家目标
    private AudioSource audioSource;
    private int currentFacingDirection = 1; // 1 表示右，-1 表示左
    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        character.maxHealth = 30;
        character.currentHealth = character.maxHealth;
        attack.damage = 8;
        attackPos.gameObject.GetComponent<Attack>().damage = 0;

        // 初始化状态机
        patrolState = new DreamCorpseElitePatrolState();
        attackState = new DreamCorpseEliteAttackState();
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        //设置朝向（根据对象的X轴缩放值决定）
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        player = GameObject.FindWithTag("Player").transform; // 根据标签锁定玩家
        if (!isDead)
        {
            currentState?.LogicUpdate();
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            currentState?.PhysicsUpdate();
            Move();
        }

    }

    public override void Move()
    {
        if (!PlayerInRangeCircle())
        {
            // 基础巡逻移动逻辑
            base.Move();
        }
        else if (PlayerInRangeCircle())
        {
            if (player != null)
            {
                // 计算敌人到玩家的方向
                Vector2 direction = (player.position - transform.position).normalized;

                // 根据方向更新 faceDir
                faceDir = new Vector3(-Mathf.Sign(direction.x), 0, 0);

                // 如果 faceDir 的方向与当前朝向不一致，更新朝向
                if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(faceDir.x))
                {
                    transform.localScale = new Vector3(faceDir.x * Mathf.Abs(transform.localScale.x),
                        transform.localScale.y,
                        transform.localScale.z);
                }

                // 移动敌人朝玩家方向
                rb.velocity = new Vector2(direction.x * currentSpeed * Time.deltaTime, rb.velocity.y);
            }
        }
    }

    public override void OnTakeDamage(Transform attackTrans)
    {
        // 受到攻击时的逻辑
        attacker = attackTrans;

        // 根据攻击者位置调整朝向
        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-2.5f, 2.5f, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(2.5f, 2.5f, 1);

        isHurt = true; // 标记受伤
        // 计算受伤的反冲方向
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;

        rb.velocity = new Vector2(0, rb.velocity.y); // 停止当前水平速度
        StartCoroutine(OnHurt(dir)); // 启动受伤协程

        // 变红效果
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor(spriteRenderer));
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

    public bool AttackPlayerInRangeCircle()
    {
        // 检测玩家是否在面朝方向的索敌范围内
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, attackLayer);

        if (hit != null)
        {
            // 计算玩家相对于怪物的方向
            Vector2 directionToPlayer = (hit.transform.position - transform.position).normalized;
            float faceDirection = -Mathf.Sign(transform.localScale.x); // 1 表示面朝右，-1 表示面朝左

            // 检测玩家是否在面朝方向
            if (Mathf.Sign(directionToPlayer.x) == faceDirection)
            {
                target = hit.transform;
                return true;
            }
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

        if (hit != null)
        {
            // 优先尝试直接获取 Character
            Character character = hit.GetComponent<Character>();

            // 如果直接获取失败，尝试从父级或子级查找
            if (character == null)
            {
                character = hit.GetComponentInParent<Character>();
            }

            if (character == null)
            {
                character = hit.GetComponentInChildren<Character>();
            }

            // 检查最终是否获取到 Character
            if (character != null)
            {
                Debug.Log("进入攻击状态");
                Attack attack = attackPos.gameObject.GetComponent<Attack>();
                if (attack != null)
                {
                    attack.damage = 8;
                    character.TakeDamage(attack);
                    Debug.Log($"Hit {character.gameObject.name} for {attack.damage} damage.");
                }

            }
        }
    }

    public void ResetAttackDamage()
    {
        attackPos.gameObject.GetComponent<Attack>().damage = 0;
    }

    public void AttackSound()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound/Jump");
        audioSource.volume = 0.15f;
        audioSource.Play();
    }

    //public bool PlayerInRangeCircle()
    //{
    //    // 检测玩家是否在索敌范围内
    //    Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, attackLayer);
    //    if (hit != null)
    //    {
    //        target = hit.transform;
    //        return true;
    //    }
    //    target = null;
    //    return false;
    //}
    private IEnumerator ResetColor(SpriteRenderer spriteRenderer)
    {
        yield return new WaitForSeconds(1); // 持续红色 0.5 秒
        spriteRenderer.color = Color.white;   // 恢复原色
    }
}
