using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCorpsePatrolState : BaseState
{

    public override void OnEnter(EnemyBase enemy)
    {
        // 初始化巡逻状态
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        // 检测是否需要翻转方向
        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.touchLeftWall || currentEnemy.physicsCheck.touchRightWall)
        {
            currentEnemy.faceDir = new Vector3(-currentEnemy.faceDir.x, 0, 0);
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}