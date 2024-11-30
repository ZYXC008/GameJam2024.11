using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeastPatrolState : BaseState
{
    private int speedCount = 0;
    public override void OnEnter(EnemyBase enemy)
    {
        Debug.Log("进入黑之兽巡逻状态");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("IsWalking", true);
        currentEnemy.GetComponent<Attack>().damage = 10;
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
                currentEnemy.currentSpeed = currentEnemy.normalSpeed;
                speedCount++;
            }

            currentEnemy.anim.speed = 1;
            currentEnemy.GetComponent<Attack>().enabled = true; // stop标志关闭时开启伤害触发脚本
        }
    }

    public override void PhysicsUpdate()
    {
        // 检测是否需要翻转方向
        if (currentEnemy is BlackBeast blackBeast && blackBeast.target == null && currentEnemy.physicsCheck.touchLeftWall)
        {
            currentEnemy.anim.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
        else if (currentEnemy is BlackBeast blackBeast1 && blackBeast1.target == null && currentEnemy.physicsCheck.touchRightWall)
        {
            currentEnemy.anim.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
    }

    public override void OnExit()
    {

    }
}
