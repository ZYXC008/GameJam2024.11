using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCorpseEliteAttackState : BaseState
{
    private bool hasAttacked;
    //private float attackTimer = 2f;
    //public float attackTime;
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        hasAttacked = false;
        //attackTime = 0;

        // 初始化速度
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("isWalking", false);
    }

    public override void LogicUpdate()
    {
        //attackTime += Time.deltaTime;
        //if (attackTime >= attackTimer)
        //{
        //    attackTime = 0;
        //    hasAttacked = false;
        //}
    }

    public override void PhysicsUpdate()
    {
        // 攻击逻辑
        if (hasAttacked && currentEnemy is DreamCorpseElite elite && elite.target != null)
        {
            currentEnemy.anim.SetTrigger("Attack");
        }
    }

    public override void OnExit()
    {
        currentEnemy.SwichState(NPCState.Patrol);
    }
}
