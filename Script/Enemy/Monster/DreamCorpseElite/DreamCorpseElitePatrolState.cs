using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCorpseElitePatrolState : BaseState
{
    private int speedCount = 0;
    private float lastCollideTime = 1f;
    public float lastCollideTimer;
    private int CollideCount;
    public override void OnEnter(EnemyBase enemy)
    {
        // 初始化巡逻状态
        currentEnemy = enemy;
        CollideCount = 0;
        lastCollideTimer = lastCollideTime + 1f; // 防止第一次碰撞时，不触发翻转
        currentEnemy.anim.SetBool("IsWalking", true);
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        if (CollideCount != 0)
        {
            lastCollideTimer += Time.deltaTime;
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
        // 检测悬崖或墙壁，进行翻转
        if (lastCollideTimer >= lastCollideTime && (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.touchLeftWall || currentEnemy.physicsCheck.touchRightWall))
        {
            lastCollideTimer = 0;
            CollideCount++;
            // 翻转方向
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
    }

    public override void OnExit()
    {
        // 离开巡逻状态
    }
}
