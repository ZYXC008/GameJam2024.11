using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeastStraightChargeState : BaseState
{
    private int speedCount = 0;
    private PhysicsCheck physicsCheck;


    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("IsWalking", false);
        currentEnemy.anim.SetTrigger("Charge");
        currentEnemy.GetComponent<Attack>().damage = 5;
        currentEnemy.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sound/Charge");
        currentEnemy.GetComponent<AudioSource>().volume = 0.7f;
        currentEnemy.GetComponent<AudioSource>().Play();
        // 获取 PhysicsCheck 组件
        physicsCheck = currentEnemy.GetComponent<PhysicsCheck>();

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
    }

    public override void PhysicsUpdate()
    {
        // 检测悬崖或墙壁，进行翻转
        if ((!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.touchLeftWall || currentEnemy.physicsCheck.touchRightWall))
        {
            // 翻转方向
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
    }

    public override void OnExit()
    {
        currentEnemy.GetComponent<BlackBeast>().attackTimer = 0; // 重置攻击计时器
    }
}