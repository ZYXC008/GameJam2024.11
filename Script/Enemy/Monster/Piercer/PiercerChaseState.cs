using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class PiercerChaseState : BaseState
{
    private int speedCount = 0;
    private bool isChasing; // 追击标志
    public float chaseTimer;
    private float chaseTime;
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        isChasing = true;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetTrigger("Attack");
    }

    public override void LogicUpdate()
    {
        if (isChasing)
        {
            chaseTimer += Time.deltaTime;
        }

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

        if (!isChasing)
        {
            // 追击结束，回到站立状态
            currentEnemy.SwichState(NPCState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {
        // 冲刺逻辑
        if (isChasing && currentEnemy is Piercer piercer && piercer.target != null && !currentEnemy.isDead)
        {
            // 追向玩家
            Vector2 chaseDirection = ((Vector2)(piercer.target.position - piercer.transform.position)).normalized;
            piercer.rb.velocity = chaseDirection * piercer.chaseSpeed;
            if (chaseTimer >= chaseTime || currentEnemy.isHurt) // 受击结束冲刺
            {

                isChasing = false;
                piercer.StartCoroutine(StandAfterChase());
            }
        }
    }

    public override void OnExit()
    {
        currentEnemy.StopMovementX();
    }

    private IEnumerator StandAfterChase()
    {
        if (currentEnemy is Piercer piercer)
        {
            if (!currentEnemy.isDead)
            {
                // 追击结束，进入站立冷却
                currentEnemy.StopMovementX();
            }
            yield return new WaitForSeconds(piercer.standTimeAfterChase);
            // 怪物没有死亡则切换回站立状态
            if (!currentEnemy.isDead)
            {
                currentEnemy.SwichState(NPCState.Patrol);
                isChasing = false;
            }
        }
    }
}