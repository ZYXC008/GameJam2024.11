using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackBeast : EnemyBase
{
    [Header("黑之兽攻击状态设置")]
    public float attackTimer = 0;
    private float attackTime = 2f;
    public int changeStateCount = 0;
    public float detectionRadius = 10f; //索敌范围
    [HideInInspector] public Transform player;       // 当前锁定的玩家
    [HideInInspector] public Transform target;
    private float distanceToPlayer; // 距离玩家的距离
    private AudioSource audioSource;
    public override void Awake()
    {
        base.Awake();
        //player = GameObject.FindWithTag("Player").transform; // 根据标签锁定玩家
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.15f;
        // 初始化状态
        patrolState = new BlackBeastPatrolState();
        chaseState = new BlackBeastStraightChargeState();
        attackState = new BlackBeastSideAttackState();
        jumpState = new BlackBeastJumpAttackState();
        currentState = patrolState;
        currentState.OnEnter(this);

        // 设置初始生命值
        character.maxHealth = 90;
        character.currentHealth = 90;
        attack.damage = 10; // 攻击伤害

    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        player = GameObject.FindWithTag("Player").transform; // 根据标签锁定玩家
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log(currentState);
        // 设置朝向（根据对象的X轴缩放值决定）
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if (!isDead)
        {
            // 看到玩家则切换攻击状态
            if (PlayerInRangeCircle())
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

            if (currentState == chaseState)
            {
                ChaseMove();
            }
        }
    }

    public override void OnTakeDamage(Transform attackTrans)
    {
        // 受到攻击时的逻辑
        attacker = attackTrans;

        // 根据攻击者位置调整朝向
        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

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

                // 获得朝向
                faceDir = new Vector2(-Mathf.Sign(direction.x), rb.velocity.y);

                // 改变黑之兽的朝向
                transform.localScale = new Vector3(faceDir.x, 1, 1);

                // 使敌人朝玩家方向移动
                rb.velocity = direction * currentSpeed * Time.deltaTime;
            }
        }
    }

    public void ChaseMove()
    {
        // 计算敌人到玩家的 X 轴方向
        float directionX = player.position.x - transform.position.x;

        // 只更新 X 轴的速度，保留 Y 轴速度
        rb.velocity = new Vector2(Mathf.Sign(directionX) * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

    public void ChangeState()
    {

        //根据与玩家的距离决定攻击方式
        //if (0 <= distanceToPlayer && distanceToPlayer <= 100000000f)
        //{
        //    changeStateCount = 0;
        //}

        if (0 <= distanceToPlayer && distanceToPlayer <= 100000000f)
        {
            changeStateCount = 1;
        }

        //if (0 <= distanceToPlayer && distanceToPlayer <= 100000000f)
        //{
        //    changeStateCount = 2;
        //}


        // 攻击cd结束后切换状态
        if (attackTimer >= attackTime)
        {
            attackTimer = 0;
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
    private IEnumerator ResetColor(SpriteRenderer spriteRenderer)
    {
        anim.speed = 0;
        yield return new WaitForSeconds(1); // 持续红色 0.5 秒
        anim.speed = 1;
        spriteRenderer.color = Color.white;   // 恢复原色
    }
    public void SideAttack()
    {
        GetComponent<Attack>().damage = 8;
    }

    public void ResetAttack()
    {
        GetComponent<Attack>().damage = 10;
    }

    public void JumpAttack()
    {
        attack.damage = 10;
        rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }

    public void ChargeAttack()
    {
        attack.damage = 5;
    }
    public void JumpAttack1()
    {
        attack.damage = 10;
        rb.AddForce(Vector2.down * 15f, ForceMode2D.Impulse);
    }

    public void JumpSound()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound/Jump");
        audioSource.volume = 0.15f;
        audioSource.Play();
    }

    public void SideAttackSound()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound/SideAttack");
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}
