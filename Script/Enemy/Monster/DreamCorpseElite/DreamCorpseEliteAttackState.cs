using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DreamCorpseEliteAttackState : BaseState
{
    private int speedCount = 0;
    private bool hasAttacked;

    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        hasAttacked = false;

        // 初始化速度
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("IsWalking", false);
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

        // 攻击逻辑
        if (!hasAttacked && currentEnemy is DreamCorpseElite elite && elite.target != null)
        {
            currentEnemy.anim.SetTrigger("Attack");
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("IsWalking", true);
    }
}
