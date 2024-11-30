using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class BlackBeastSideAttackState : BaseState
{
    private int speedCount = 0;
    //private bool hasAttacked;
    //public float attackTimer;
    //private float attackTime;
    //private int attackCount = 0; // 记录攻击方向和攻击次数
    public override void OnEnter(EnemyBase enemy)
    {
        Debug.Log("进入黑之兽侧边攻击状态");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
        //如果动画没有攻击时间则用这个
        //attackTimer = attackTime; // 先攻击一次,第二次攻击有间隔
        //attackCount = 0;
        currentEnemy.anim.SetBool("IsWalking", false);
        currentEnemy.GetComponent<Attack>().damage = 8;
        currentEnemy.anim.SetTrigger("Attack");
        //hasAttacked = false;
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
                currentEnemy.currentSpeed = 0;
                speedCount++;
            }

            currentEnemy.anim.speed = 1;
            currentEnemy.GetComponent<Attack>().enabled = true; // stop标志关闭时开启伤害触发脚本
        }

        //if (attackTimer >= attackTime)
        //{
        //    attackTimer = 0;
        //    hasAttacked = false;
        //}

        //if (hasAttacked == false)
        //{
        //    hasAttacked = true;
        //}

        //attackTimer += Time.deltaTime;
        //if (!hasAttacked && attackCount < 2 && attackTime >= attackTimer && !currentEnemy.isDead && !currentEnemy.character.stop) //只能攻击两次 且攻击两边
        //{
        //    attackTime = 0;
        //    currentEnemy.anim.SetTrigger("Attack");
        //    // 停止角色移动
        //    currentEnemy.StopMovement();

        //    // 切换攻击方向
        //    currentEnemy.GetComponent<SpriteRenderer>().flipX = true;
        //    //currentEnemy.faceDir.x = attackCount % 2 == 0 ? -1 : 1;

        //    // 更新角色朝向
        //    //currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);


        //    // 增加攻击计数
        //    attackCount++;
        //}




    }


    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
