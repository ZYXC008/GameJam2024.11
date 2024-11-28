using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeastJumpAttackState : BaseState
{
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("isWalking", false);
        currentEnemy.anim.SetTrigger("JumpAttack");
        currentEnemy.rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }

    public override void LogicUpdate() { }

    public override void PhysicsUpdate()
    {

        if (currentEnemy.physicsCheck.isGround && !currentEnemy.isDead)
        {
            // 当落地时触发攻击效果
            currentEnemy.GetComponent<Attack>().damage = 10;
        }
    }

    public override void OnExit()
    {
        // 攻击动画结束后等待1s并且切换到巡逻状态
        currentEnemy.StartCoroutine(StopAndChange());
    }

    IEnumerator StopAndChange()
    {
        // 停止移动
        currentEnemy.StopMovement();
        // 等待1秒
        yield return new WaitForSeconds(1f);

        // 切换到巡逻状态
        currentEnemy.SwichState(NPCState.Patrol);
    }
}