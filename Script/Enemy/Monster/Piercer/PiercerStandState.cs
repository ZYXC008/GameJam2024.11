using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercerStandState : BaseState
{
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("IsWalking", true);
        currentEnemy.currentSpeed = 0; // 停止移动
    }

    public override void LogicUpdate()
    {
        if (currentEnemy is Piercer piercer && piercer.PlayerInRangeCircle())
        {
            // 如果玩家进入范围，切换到冲刺状态
            currentEnemy.SwichState(NPCState.Chase);
        }
    }

    public override void PhysicsUpdate()
    {
        // 站立不动
        currentEnemy.rb.velocity = Vector2.zero;
    }

    public override void OnExit()
    {
        // 离开站立状态
    }
}
