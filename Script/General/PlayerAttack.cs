using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    public PlayerWeaponController controller;


    private void Update()
    {
        damage = controller.weaponAttack.damage;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
