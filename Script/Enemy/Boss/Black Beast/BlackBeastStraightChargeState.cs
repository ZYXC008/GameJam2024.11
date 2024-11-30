using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeastStraightChargeState : BaseState
{
    private int speedCount = 0;
    private PhysicsCheck physicsCheck;


    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;

        currentEnemy.anim.SetBool("IsWalking", false);
        currentEnemy.anim.SetTrigger("Charge");
        currentEnemy.GetComponent<Attack>().damage = 5;
        // 获取 PhysicsCheck 组件
        physicsCheck = currentEnemy.GetComponent<PhysicsCheck>();

    }

    public override void LogicUpdate()
    {
        if (currentEnemy.character.stop && !currentEnemy.isDead)
        {
            currentEnemy.StopMovement();
            currentEnemy.anim.speed = 0f;  // 设置 speed 为 0，暂停动画
            currentEnemy.GetComponent<Attack>().enabled = false; // 关闭伤害触发脚本
        }

        if (!currentEnemy.character.stop && currentEnemy.isDead)
        {
            if (speedCount == 0)
            {
                currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
                speedCount++;
            }

            currentEnemy.anim.speed = 1;
            currentEnemy.GetComponent<Attack>().enabled = true; // stop标志关闭时开启伤害触发脚本
        }
    }

    public override void PhysicsUpdate()
    {
        // 检测是否撞到墙 撞到则停止1s并且切换到巡逻状态
        if (physicsCheck != null && (physicsCheck.touchLeftWall || physicsCheck.touchRightWall))
        {
            currentEnemy.StartCoroutine(StopAndChange());
        }
    }

    public override void OnExit()
    {

    }

    IEnumerator StopAndChange()
    {
        if (!currentEnemy.isDead)
        {
            // 停止移动
            currentEnemy.StopMovement();
        }
        // 等待1秒
        yield return new WaitForSeconds(1f);

        if (!currentEnemy.isDead)
        {
            currentEnemy.GetComponent<BlackBeast>().attackTimer = 0; // 重置攻击计时器
            // 切换到巡逻状态
            currentEnemy.SwichState(NPCState.Patrol);
        }
    }
}