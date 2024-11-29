using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
public class EnemyBase : MonoBehaviour
{
    // Rigidbody2D组件，用于处理物理相关的操作
    public Rigidbody2D rb;
    [HideInInspector] public Character character;

    // 动画控制器
    [HideInInspector] public Animator anim;

    // 自定义的检测地面等物理状态的组件
    [HideInInspector] public PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed;    // 普通巡逻时的移动速度
    public float chaseSpeed;     // 追击目标时的移动速度
    public float currentSpeed; // 当前的移动速度（动态切换）
    public float hurtForce;      // 受伤时的反冲力度
    public Vector3 faceDir;      // 朝向的方向向量

    [HideInInspector] public Transform attacker; // 最近攻击敌人的对象

    [Header("计时器")]
    public float waitTime;       // 巡逻等待的总时长
    public float waitTimeCounter; // 巡逻等待计时器
    public bool wait = false;    // 是否处于等待状态
    public float lostTime;       // 目标丢失后的追击持续时间
    public float lostTimeCounter; // 目标丢失计时器

    [Header("检测")]
    public Vector2 centerOfferset; // 检测框的中心偏移
    public Vector2 checkSize;      // 检测框的大小
    public float checkDistance;    // 检测的距离
    public LayerMask attackLayer;  // 用于检测的层级（玩家或其他目标）

    [Header("状态")]
    public bool isHurt;  // 是否处于受伤状态
    public bool isDead;  // 是否已经死亡

    // 当前状态和子状态
    public BaseState currentState; // 当前敌人状态
    protected BaseState patrolState; // 巡逻状态
    protected BaseState chaseState;  // 追击状态
    protected BaseState attackState; // 黑之兽左右攻击状态
    protected BaseState jumpState; // 黑之兽跳跃攻击状态

    public virtual void Awake()
    {

        // 获取必要的组件
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        character = GetComponent<Character>();

        // 初始化基本参数
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
        physicsCheck.isGround = true; // 默认认为在地面上


    }



    private void OnEnable()
    {
        // 激活时进入初始状态（巡逻）
        //currentState = patrolState;
        //currentState.OnEnter(this); // 调用状态的进入方法
    }

    private void Update()
    {
        // 设置朝向（根据对象的X轴缩放值决定）
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        // 状态逻辑更新
        currentState.LogicUpdate();

        // 计时器更新（用于巡逻等待、追击超时等逻辑）
        TimeCounter();
    }

    private void FixedUpdate()
    {
        // 当不在受伤、死亡或等待状态时，执行移动
        if (!isHurt && !isDead && !wait)
        {
            Move();
        }

        // 状态的物理更新
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        // 禁用时退出当前状态
        currentState.OnExit();
    }

    #region 计时器
    public void TimeCounter()
    {
        // 巡逻等待计时器
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;

                // 等待结束后切换朝向
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }

        // 追击丢失计时器
        if (!FoundPlayer())
        {
            if (lostTimeCounter > 0)
                lostTimeCounter -= Time.deltaTime;
        }
        else
        {
            lostTimeCounter = lostTime; // 每次发现玩家时重置计时
        }
    }
    #endregion

    public virtual void Move()
    {
        // 控制敌人水平移动，速度由当前状态决定
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    public bool FoundPlayer()
    {
        // 使用BoxCast检测玩家是否在攻击范围内
        return Physics2D.BoxCast(
            transform.position + (Vector3)centerOfferset,
            checkSize,
            0,
            faceDir,
            checkDistance,
            attackLayer
        );
    }

    public void SwichState(NPCState state)
    {
        // 切换状态（巡逻、追击）
        BaseState newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Attack => attackState,
            NPCState.Jump => jumpState,
            _ => null
        };
        currentState.OnExit();   // 退出当前状态
        currentState = newState; // 切换到新状态
        currentState.OnEnter(this); // 进入新状态
    }

    public void OnTakeDamage(Transform attackTrans)
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

        // 回到当前动画的第一帧
        anim.Play(anim.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0);

        // 变红效果
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor(spriteRenderer));
    }

    IEnumerator OnHurt(Vector2 dir)
    {
        // 应用反冲力
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);

        // 受伤状态持续时间
        yield return new WaitForSeconds(0.5f);
        isHurt = false; // 恢复正常状态
    }

    public virtual void OnDie()
    {
        // 敌人死亡逻辑
        gameObject.layer = 2; // 将对象设置为忽略层
        anim.SetBool("Dead", true); // 播放死亡动画
        isDead = true; // 标记为死亡状态
        StartCoroutine(DestroyAfterAnimation()); // 启动销毁协程
    }

    public IEnumerator DestroyAfterAnimation()
    {
        // 获取当前动画的长度
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // 等待动画播放完毕
        yield return new WaitForSeconds(animationLength);

        // 销毁当前游戏对象
        Destroy(gameObject);
    }
    public virtual void StopMovement()
    {
        rb.velocity = Vector2.zero; // 让怪物静止
    }
    public virtual void StopMovementX()
    {
        rb.velocity = new Vector2(0, rb.velocity.y); // 停止当前水平速度
    }
    private IEnumerator ResetColor(SpriteRenderer spriteRenderer)
    {
        anim.speed = 0;
        yield return new WaitForSeconds(0.5f); // 持续红色 0.5 秒
        anim.speed = 1;
        spriteRenderer.color = Color.white;   // 恢复原色
    }
}
