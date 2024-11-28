using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAddDamage : MonoBehaviour, IInteractable
{
    public int buffTimes;
    public int buffDamage;
    public void TriggerAction(Character character)
    {
        TakeItem(character);
    }

    public void TakeItem(Character character)
    {
        character.GetComponent<PlayerWeaponController>().buffTimes = buffTimes;
        character.GetComponent<PlayerWeaponController>().buffDamage += buffDamage;
        character.GetComponent<PlayerWeaponController>().isBuffed = false;
        gameObject.SetActive(false);
    }
}
