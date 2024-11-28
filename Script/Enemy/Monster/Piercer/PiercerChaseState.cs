using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class PiercerChaseState : BaseState
{
    private bool isChasing; // 追击标志
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        isChasing = true;
        currentEnemy.anim.SetTrigger("Attack");
    }

    public override void LogicUpdate()
    {

        if (!isChasing)
        {
            // 追击结束，回到站立状态
            currentEnemy.SwichState(NPCState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {
        // 冲刺逻辑
        if (isChasing && currentEnemy is Piercer piercer && piercer.target != null)
        {
            // 追向玩家
            Vector2 chaseDirection = ((Vector2)(piercer.target.position - piercer.transform.position)).normalized;
            piercer.rb.velocity = chaseDirection * piercer.chaseSpeed;
            if (piercer.target == null || currentEnemy.isHurt || currentEnemy.isDead) // 追击持续时间,受击或死亡结束冲刺
            {
                isChasing = false;
                piercer.StartCoroutine(StandAfterChase());
            }
        }
    }

    public override void OnExit()
    {
        // 离开追击状态，重置速度
        currentEnemy.rb.velocity = Vector2.zero;
    }

    private IEnumerator StandAfterChase()
    {
        if (currentEnemy is Piercer piercer)
        {
            // 追击结束，进入站立冷却
            piercer.rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(piercer.standTimeAfterChase);
            currentEnemy.SwichState(NPCState.Patrol);
            isChasing = false;
        }
    }
}