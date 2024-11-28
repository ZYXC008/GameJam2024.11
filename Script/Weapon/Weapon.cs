using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable
{
    public int weaponTag;
    public int damage;
    public Sprite weaponImage;
    public PlayerWeaponController weaponController;
    public GameObject weapon;

    public void TakeItem(Character character)
    {
        weaponController = character.GetComponent<PlayerWeaponController>();
        weaponController.weaponPrefebs.Add(weapon);
        weapon.SetActive(false);
    }

    public void TriggerAction(Character character)
    {
        TakeItem(character);
    }
}
