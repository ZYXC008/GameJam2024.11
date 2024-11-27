using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DreamCorpse : EnemyBase
{

    public override void Awake()
    {
        base.Awake();

        // 初始化巡逻状态
        currentSpeed = normalSpeed;
        patrolState = new DreamCorpsePatrolState();

        // 初始化数据
        this.GetComponent<Attack>().damage = 4;
        character.maxHealth = 12;
        character.currentHealth = character.maxHealth;
    }

    private void Update()
    {
        if (!isDead && !isHurt)
        {
            // 状态更新逻辑
            currentState?.LogicUpdate();
        }
    }

    void FixedUpdate()
    {
        if (!isDead && !isHurt)
        {
            Move();
        }
        currentState?.PhysicsUpdate();
    }

}