using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable
{
    public WeaponType weaponTag;
    public int damage;
    public Sprite weaponImage;
    public PlayerWeaponController weaponController;
    public GameObject weapon;

    public void TakeItem(Character character)
    {   weaponController = character.GetComponent<PlayerWeaponController>();
        if(weaponTag != WeaponType.StopEnemy)
            weaponController.weaponPrefebs.Add(weapon);
        else
            weaponController.skillPrefebs.Add(weapon);
        weapon.SetActive(false);
    }

    public void TriggerAction(Character character)
    {
        TakeItem(character);
    }

    public void Skill(Character character)
    {
        if(weaponTag == WeaponType.StopEnemy)
        {
            //¶¨×¡µÐÈË
            character.stop = true;
            character.stopCounter = character.stopDuration;
        }
    }
}
