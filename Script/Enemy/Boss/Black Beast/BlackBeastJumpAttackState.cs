using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeastJumpAttackState : BaseState
{
    private Vector2 currentSpeed;
    private int speedCount = 0;
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("IsWalking", false);
        currentEnemy.anim.SetTrigger("JumpAttack");
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.character.stop && !currentEnemy.isDead)
        {
            // 刚进入停止状态记录当前速度
            if (speedCount == 0)
            {
                currentSpeed = currentEnemy.GetComponent<Rigidbody2D>().velocity;
                speedCount++;
            }
            currentEnemy.StopMovement();
            currentEnemy.anim.speed = 0f;  // 设置 speed 为 0，暂停动画
            currentEnemy.GetComponent<Attack>().enabled = false; // 关闭伤害触发脚本
        }

        if (!currentEnemy.character.stop && currentEnemy.isDead)
        {
            if (speedCount == 1)
            {
                currentEnemy.GetComponent<Rigidbody2D>().velocity = currentSpeed;
                speedCount++;
            }

            currentEnemy.anim.speed = 1;
            currentEnemy.GetComponent<Attack>().enabled = true; // stop标志关闭时开启伤害触发脚本
        }
    }

    public override void PhysicsUpdate()
    {
        if (currentEnemy.physicsCheck.isGround && !currentEnemy.isDead)
        {
            // 当落地时触发攻击效果
            currentEnemy.GetComponent<Attack>().damage = 10;
        }
    }

    public override void OnExit()
    {
        //// 攻击动画结束后等待1s并且切换到巡逻状态
        //currentEnemy.StartCoroutine(StopAndChange());
        currentEnemy.GetComponent<BlackBeast>().attackTimer = 0; // 重置攻击计时器
    }

    //IEnumerator StopAndChange()
    //{
    //    if (!currentEnemy.isDead)
    //    {
    //        // 停止移动
    //        currentEnemy.StopMovement();
    //    }
    //    // 等待1秒
    //    yield return new WaitForSeconds(1f);

    //    if (!currentEnemy.isDead)
    //    {
    //        currentEnemy.GetComponent<BlackBeast>().attackTimer = 0; // 重置攻击计时器
    //        // 切换到巡逻状态
    //        currentEnemy.SwichState(NPCState.Patrol);
    //    }
    //}
}