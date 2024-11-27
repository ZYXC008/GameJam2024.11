using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFlyPatrolState : BaseState
{

    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("IsWalking", true);
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
