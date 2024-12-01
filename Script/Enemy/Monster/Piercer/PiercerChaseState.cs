using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
        //Debug.Log("Piercer Chase");

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

                // 计算玩家与敌人之间的 X 轴方向
                float directionX = piercer.target.position.x - piercer.transform.position.x;

                // 根据 X 轴方向更新 faceDir
                piercer.faceDir = new Vector3(-Mathf.Sign(directionX), 0, 0);

                // 如果 faceDir 的方向与当前朝向不一致，更新朝向
                if (Mathf.Sign(piercer.transform.localScale.x) != Mathf.Sign(piercer.faceDir.x))
                {
                    piercer.transform.localScale = new Vector3(piercer.faceDir.x * Mathf.Abs(piercer.transform.localScale.x),
                        piercer.transform.localScale.y,
                        piercer.transform.localScale.z);
                }

                // 只更新 X 轴的速度，保持 Y 轴速度不变
                piercer.rb.velocity = new Vector2(Mathf.Sign(directionX) * piercer.currentSpeed, piercer.rb.velocity.y);
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