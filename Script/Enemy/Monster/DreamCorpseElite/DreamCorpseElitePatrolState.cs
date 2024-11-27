using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCorpseElitePatrolState : BaseState
{
    public override void OnEnter(EnemyBase enemy)
    {
        // 初始化巡逻状态
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("isWalking", true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy is DreamCorpseElite elite && elite.PlayerInRangeCircle())
        {
            // 如果玩家进入范围，切换到攻击状态
            currentEnemy.SwichState(NPCState.Attack);
        }
    }

    public override void PhysicsUpdate()
    {
        // 检测是否需要翻转方向
        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.touchLeftWall || currentEnemy.physicsCheck.touchRightWall)
        {
            currentEnemy.faceDir = new Vector3(-currentEnemy.faceDir.x, 0, 0);
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
    }

    public override void OnExit()
    {
        // 离开巡逻状态
    }
}
