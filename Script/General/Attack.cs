using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public PlayerWeaponController controller;
    private void Awake()
    {
        controller = GetComponent<PlayerWeaponController>();
        damage = controller.weaponSkill.damage;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
