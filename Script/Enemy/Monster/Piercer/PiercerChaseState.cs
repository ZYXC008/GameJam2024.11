using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class PiercerChaseState : BaseState
{
    private int speedCount = 0;
    private bool isChasing; // 追击标志
    public float chaseTimer = 0;
    private float chaseTime = 2f;
    private bool attackTriggered = false; // 确保只触发一次攻击
    private bool isChaseCoroutineRunning = false; // 防止协程重复运行
    private bool velocitySet;
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        speedCount = 0;
        chaseTimer = 0; // 重置追击计时
        isChasing = true;
        velocitySet = false;
        attackTriggered = false; // 重置攻击触发状态
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
    }

    public override void LogicUpdate()
    {
        Debug.Log("Piercer Chase");

        if (isChasing && !currentEnemy.isHurt)
        {
            chaseTimer += Time.deltaTime;
        }

        if (currentEnemy.isHurt)
        {
            chaseTimer = 0;
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
    }

    public override void PhysicsUpdate()
    {
        if (isChasing && currentEnemy is Piercer piercer && piercer.target != null && !currentEnemy.isDead)
        {
            if (chaseTimer < chaseTime)
            {
                // 触发攻击动画，只触发一次
                if (!attackTriggered)
                {
                    currentEnemy.anim.SetTrigger("Attack");
                    attackTriggered = true; // 确保只触发一次
                }

                // 持续追向玩家，仅沿 X 轴方向追击
                Vector2 chaseDirection = new Vector2(piercer.target.position.x - piercer.transform.position.x, 0).normalized;
                piercer.rb.velocity = chaseDirection * piercer.chaseSpeed;
            }
            else if (chaseTimer >= chaseTime && !isChaseCoroutineRunning)
            {
                piercer.StartCoroutine(StandAfterChase());
            }
        }
    }

    //public override void PhysicsUpdate()
    //{
    //    if (isChasing && currentEnemy is Piercer piercer && piercer.target != null && !currentEnemy.isDead)
    //    {
    //        if (chaseTimer < chaseTime)
    //        {
    //            // 触发攻击动画，只触发一次
    //            if (!attackTriggered)
    //            {
    //                currentEnemy.anim.SetTrigger("Attack");
    //                attackTriggered = true; // 确保只触发一次
    //            }

    //            // 仅第一次追击时设置速度
    //            if (!velocitySet)
    //            {
    //                Vector2 chaseDirection = new Vector2(piercer.target.position.x - piercer.transform.position.x, 0).normalized;
    //                piercer.rb.velocity = chaseDirection * piercer.chaseSpeed;
    //                velocitySet = true; // 确保速度只设置一次
    //            }
    //        }
    //        else if (chaseTimer >= chaseTime && !isChaseCoroutineRunning)
    //        {
    //            piercer.StartCoroutine(StandAfterChase());
    //            velocitySet = false; // 重置标记，以便下一次追击可以重新设置速度
    //        }
    //    }
    //}

    public override void OnExit()
    {
        isChasing = false; // 明确标记追击结束
        isChaseCoroutineRunning = false; // 重置协程运行标志
    }

    private IEnumerator StandAfterChase()
    {
        isChaseCoroutineRunning = true; // 标记协程已运行
        currentEnemy.StopMovement();
        yield return new WaitForSeconds(2f);

        if (!currentEnemy.isDead)
        {
            currentEnemy.SwichState(NPCState.Patrol);
        }

    }
}