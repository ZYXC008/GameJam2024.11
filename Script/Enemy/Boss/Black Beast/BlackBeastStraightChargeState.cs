using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeastStraightChargeState : BaseState
{

    private PhysicsCheck physicsCheck;


    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;

        currentEnemy.anim.SetBool("isWalking", false);
        currentEnemy.anim.SetTrigger("Charge");
        currentEnemy.GetComponent<Attack>().damage = 5;
        // 获取 PhysicsCheck 组件
        physicsCheck = currentEnemy.GetComponent<PhysicsCheck>();

    }

    public override void LogicUpdate()
    {
        // 执行冲撞
        currentEnemy.rb.velocity = new Vector2(currentEnemy.faceDir.x * currentEnemy.chaseSpeed, 0);
    }

    public override void PhysicsUpdate()
    {
        // 检测是否撞到墙 撞到则停止1s并且切换到巡逻状态
        if (physicsCheck != null && (physicsCheck.touchLeftWall || physicsCheck.touchRightWall))
        {
            currentEnemy.StartCoroutine(StopAndChange());
        }
    }

    public override void OnExit()
    {

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