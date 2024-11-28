using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class BlackBeastSideAttackState : BaseState
{
    private bool hasAttacked;
    public float attackTimer = 1f;
    private float attackTime;
    private int attackCount = 0; // 记录攻击方向和攻击次数
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        // 如果动画没有攻击时间则用这个
        // attackTimer = attackTime; // 先攻击一次,第二次攻击有间隔
        attackCount = 0;
        currentEnemy.anim.SetBool("isWalking", false);
        currentEnemy.anim.SetTrigger("Attack");
        currentEnemy.GetComponent<Attack>().damage = 8;
        hasAttacked = false;
    }

    public override void LogicUpdate()
    {
        attackTimer += Time.deltaTime;
        if (!hasAttacked && attackCount < 2 && attackTime >= attackTimer) //只能攻击两次 且攻击两边
        {
            attackTime = 0;
            // 停止角色移动
            currentEnemy.StopMovement();

            // 切换攻击方向
            currentEnemy.faceDir.x = attackCount % 2 == 0 ? -1 : 1;

            // 更新角色朝向
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);

            // 增加攻击计数
            attackCount++;
        }

        if (hasAttacked == false)
        {
            hasAttacked = true;
        }

        if (attackTimer >= attackTime)
        {
            attackTimer = 0;
            hasAttacked = false;
        }
    }


    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        // 退出状态时重置攻击计数
        attackCount = 0;
        // 攻击动画结束后等待1s并且切换到巡逻状态
        currentEnemy.StartCoroutine(StopAndChange());
    }

    IEnumerator StopAndChange()
    {

        currentEnemy.StopMovement();
        // 等待1秒
        yield return new WaitForSeconds(1f);

        // 切换到巡逻状态
        currentEnemy.SwichState(NPCState.Patrol);
    }
}
