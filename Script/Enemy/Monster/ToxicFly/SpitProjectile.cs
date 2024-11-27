using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitProjectile : MonoBehaviour
{
    private Transform target;

    private void Awake()
    {
        GetComponent<Attack>().damage = 4;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果飞沫与玩家碰撞，造成伤害
        if (collision.transform == target)
        {
            Destroy(gameObject); // 播放飞沫消失动画后销毁
        }
    }
}