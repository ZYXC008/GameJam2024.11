using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeastPatrolState : BaseState
{
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("isWalking", true);
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        // 没看到玩家且撞墙则转向
        if (!currentEnemy.FoundPlayer() && (currentEnemy.physicsCheck.touchLeftWall || currentEnemy.physicsCheck.touchRightWall))
        {
            currentEnemy.faceDir = new Vector3(-currentEnemy.faceDir.x, 0, 0);
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
    }

    public override void OnExit()
    {

    }
}
