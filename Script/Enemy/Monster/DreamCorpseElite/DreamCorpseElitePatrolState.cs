using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCorpseElitePatrolState : BaseState
{
    private int speedCount = 0;
    public override void OnEnter(EnemyBase enemy)
    {
        // 初始化巡逻状态
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("IsWalking", true);
    }

    public override void LogicUpdate()
    {
        Debug.Log("Patrol");
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
                currentEnemy.currentSpeed = currentEnemy.normalSpeed;
                speedCount++;
            }
            currentEnemy.anim.speed = 1;
            currentEnemy.GetComponent<Attack>().enabled = true; // stop标志关闭时开启伤害触发脚本
        }

        if (currentEnemy is DreamCorpseElite elite && elite.PlayerInRangeCircle())
        {
            // 如果玩家进入范围，切换到攻击状态
            currentEnemy.SwichState(NPCState.Attack);
        }
    }

    public override void PhysicsUpdate()
    {
        // 检测是否需要翻转方向
        if (currentEnemy.physicsCheck.touchLeftWall)
        {
            currentEnemy.anim.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
        else if (currentEnemy.physicsCheck.touchRightWall)
        {
            currentEnemy.anim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
    }

    public override void OnExit()
    {
        // 离开巡逻状态
    }
}
