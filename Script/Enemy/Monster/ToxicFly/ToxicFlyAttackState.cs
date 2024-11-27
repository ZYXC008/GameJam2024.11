using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//public class ToxicFlyAttackState : BaseState
//{
//    public float attackTime;
//    private int attackCount = 1;
//    private float attackTimer = 1f; // 每颗飞沫的攻击间隔时间
//    public override void OnEnter(EnemyBase enemy)
//    {
//        currentEnemy = enemy;

//        currentEnemy.anim.SetBool("IsWalking", false);
//        currentEnemy.anim.SetTrigger("Attack"); //进入攻击状态则先攻击一次
//        // 获得攻击时的伤害
//        currentEnemy.GetComponent<Attack>().damage = 2;
//    }

//    public override void LogicUpdate()
//    {

//    }

//    public override void PhysicsUpdate()
//    {

//    }

//    public override void OnExit()
//    {
//        currentEnemy.anim.SetBool("IsWalking", true);
//        currentEnemy.SwichState(NPCState.Patrol);
//    }
//}

public class ToxicFlyAttackState : BaseState
{
    private bool hasAttacked;
    private float attackTimer;
    private int attackCount = 0;
    private float attackInterval = 2f; // 飞沫攻击间隔
    private float attackCooldown = 0.5f; // 攻击准备时间

    private Transform playerTarget;


    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        // 看动画里有没有
        //attackTimer = 0;
        //attackCount = 1;
        hasAttacked = false;
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void LogicUpdate()
    {
        if (hasAttacked) return;

        // 攻击逻辑：等待 0.5 秒钟后发射飞沫
        //attackTimer += Time.deltaTime;
        //if (attackTimer >= attackCooldown)
        //{
        //    currentEnemy.StartCoroutine(AttackRoutine());
        //    hasAttacked = true;
        //}
    }

    public override void PhysicsUpdate()
    {
        // 碰撞逻辑在子弹里面处理
    }

    public override void OnExit()
    {
        // 退出攻击状态时
        //attackTimer = 0;
        attackCount = 0;
    }

    private void AttackRoutine()
    {
        //while (attackCount < 2) // 攻击两次
        //{
        //    // 每次攻击时增加计数，并且处理飞沫射击
        //    attackCount++;
        //    // 向玩家发射飞沫攻击
        //    ShootSpitAtPlayer();
        //    yield return new WaitForSeconds(attackInterval); // 每颗飞沫攻击间隔
        //}

        // 向玩家发射飞沫攻击,在关键帧中加入
        ShootSpitAtPlayer();
        attackCount++;
        // 完成攻击后切换状态为巡逻
        if (attackCount == 2)
        {
            currentEnemy.SwichState(NPCState.Patrol);
        }
    }

    private void ShootSpitAtPlayer()
    {
        // 创建飞沫（可以是一个子弹或者射线）
        // 假设飞沫是通过一个简单的物理物体射出
        GameObject spitProjectile = GameObject.Instantiate(Resources.Load<GameObject>("Monster/ToxicFly/spitProjectile"));
        spitProjectile.transform.position = currentEnemy.transform.position;
        Vector2 direction = (playerTarget.position - currentEnemy.transform.position).normalized;
        spitProjectile.GetComponent<Rigidbody2D>().velocity = direction * 10f; // 10f 为飞沫的速度

        // 攻击对象
        spitProjectile.GetComponent<SpitProjectile>().SetTarget(playerTarget);
    }
}