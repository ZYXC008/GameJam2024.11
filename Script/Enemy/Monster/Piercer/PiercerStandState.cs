using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercerStandState : BaseState
{
    private int speedCount = 0;
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("IsWalking", true);
        currentEnemy.currentSpeed = currentEnemy.normalSpeed; // 初始化速度
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
                currentEnemy.currentSpeed = currentEnemy.normalSpeed;
                speedCount++;
            }
            currentEnemy.anim.speed = 1;
            currentEnemy.GetComponent<Attack>().enabled = true; // stop标志关闭时开启伤害触发脚本
        }

        if (currentEnemy is Piercer piercer && piercer.PlayerInRangeCircle())
        {
            // 如果玩家进入范围，切换到冲刺状态
            currentEnemy.SwichState(NPCState.Chase);
        }

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        // 离开站立状态
    }
}
