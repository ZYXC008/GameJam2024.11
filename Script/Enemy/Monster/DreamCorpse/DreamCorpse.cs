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

        patrolState = new DreamCorpsePatrolState();
        currentState = patrolState;
        currentState.OnEnter(this);

        // 初始化数据
        attack.damage = 4;
        character.maxHealth = 12;
        character.currentHealth = character.maxHealth;
    }

    private void Update()
    {
        // 设置朝向（根据对象的X轴缩放值决定）
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if (!isDead)
        {
            // 状态更新逻辑
            currentState?.LogicUpdate();
        }
    }

    void FixedUpdate()
    {
        if (!isDead && !isHurt)
        {
            currentState?.PhysicsUpdate();
            Move();
        }

    }

}