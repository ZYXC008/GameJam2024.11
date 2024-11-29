using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    [Header("ÎäÆ÷ÁÐ±í")]
    public List<GameObject> weaponPrefebs = new List<GameObject>();
    public List<GameObject> skillPrefebs = new List<GameObject>();

    [Header("ÎäÆ÷ÐòºÅ")]
    public int weaponCount = 0;
    public int skillCount = 0;

    [Header("Buff")]
    public int buffTimes;
    public int buffDamage;
    public bool isBuffed;
    public int damage;

    public Weapon weaponAttack;
    public Weapon skillAttack;

    private void Awake()
    {
        weaponAttack = weaponPrefebs[weaponCount]?.GetComponent<Weapon>();
        inputControl = new PlayerInputControl();
        inputControl.Gameplay.SwitchWeaponUp.started += SwitchWeaponUp;
        inputControl.Gameplay.SwitchWeaponDown.started += SwitchWeaponDown;
        inputControl.Gameplay.SwitchSkillUp.started += SwitchSkillUp;
        inputControl.Gameplay.SwitchSkillDown.started += SwitchSkillDown;
    }
    private void Update()
    {
        if (!isBuffed)
        {
            if(buffDamage > 0)
                weaponAttack.damage = damage;
            weaponAttack.damage += buffDamage;
            isBuffed = true;
        }
        if(buffTimes == 0 && isBuffed)
        {
            weaponAttack.damage = damage;
            buffDamage = 0;
            isBuffed= false;
        }
    }
    private void OnEnable()
    {
        inputControl.Enable();
        damage = weaponAttack.damage;
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
        weaponAttack = weaponPrefebs[weaponCount].GetComponent<Weapon>();
        damage = weaponAttack.damage;
        isBuffed = false;
    } 
    private void SwitchWeaponDown(InputAction.CallbackContext context)
    {
        weaponCount = (weaponCount + 1) % weaponPrefebs.Count;
        weaponAttack = weaponPrefebs[weaponCount].GetComponent<Weapon>();
        damage = weaponAttack.damage;
        isBuffed = false;
    }
    private void SwitchSkillUp(InputAction.CallbackContext context)
    {
        if(skillPrefebs.Count > 0)
        {
            if (skillCount == 0)
            {
                skillCount = skillPrefebs.Count - 1;
            }
            else
            {
                skillCount--;
            }
            skillAttack = skillPrefebs[skillCount].GetComponent<Weapon>();

        }
        
    }
    private void SwitchSkillDown(InputAction.CallbackContext context)
    {
        if (skillPrefebs.Count > 0)
        {
            skillCount = (skillCount + 1) % skillPrefebs.Count;
            skillAttack = skillPrefebs[skillCount].GetComponent<Weapon>();
        }
        
    }
}