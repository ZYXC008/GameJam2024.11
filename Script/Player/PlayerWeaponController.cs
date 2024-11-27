using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    [Header("ÎäÆ÷ÁÐ±í")]
    public List<GameObject> weaponPrefebs = new List<GameObject>();

    [Header("ÎäÆ÷ÐòºÅ")]
    public int weaponCount = 0;

    public Weapon weaponSkill;

    private void Awake()
    {
        weaponSkill = weaponPrefebs[weaponCount]?.GetComponent<Weapon>();
        inputControl = new PlayerInputControl();
        inputControl.Gameplay.SwitchWeaponUp.started += SwitchWeaponUp;
        inputControl.Gameplay.SwitchWeaponDown.started += SwitchWeaponDown;
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void SwitchWeaponUp(InputAction.CallbackContext context)
    {
        if (weaponCount == 0)
        {
            weaponCount = weaponPrefebs.Count - 1;
        }
        else
        {
            weaponCount--;
        }
        weaponSkill = weaponPrefebs[weaponCount].GetComponent<Weapon>();
    } 
    private void SwitchWeaponDown(InputAction.CallbackContext context)
    {
        weaponCount = (weaponCount + 1) % weaponPrefebs.Count;
        weaponSkill = weaponPrefebs[weaponCount].GetComponent<Weapon>();
    }
}