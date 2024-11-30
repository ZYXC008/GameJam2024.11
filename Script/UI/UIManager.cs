using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public PlayerStuaBar playerStuaBar;
    public PlayerWeaponController playerWeaponController;

    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO loadEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        loadEvent.LoadRequestEvent += OnLoadEvent;
    }
    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEvent.LoadRequestEvent -= OnLoadEvent;
    }
    private void Update()
    {
        if(playerWeaponController.weaponAttack.weaponTag == WeaponType.Normal)
        {
            playerStuaBar.Weapon_Normal.gameObject.SetActive(true);
            playerStuaBar.Weapon_BigArea.gameObject.SetActive(false);
        }
        if (playerWeaponController.weaponAttack.weaponTag == WeaponType.BigArea)
        {
            playerStuaBar.Weapon_Normal.gameObject.SetActive(false);
            playerStuaBar.Weapon_BigArea.gameObject.SetActive(true);
        }
        if(playerWeaponController.skillPrefebs.Count > 0)
        {
            playerStuaBar.Skill_Stop.gameObject.SetActive(true);
        }
    }
    private void OnLoadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMemu = sceneToLoad.sceneType == SceneType.Menu;
        playerStuaBar.gameObject.SetActive(!isMemu);
    }

    private void OnHealthEvent(Character character)
    {
        var persontageHP = character.currentHealth / character.maxHealth;
        var persontageMP = character.currentMP / character.maxMP;
        playerStuaBar.OnStatusChage(persontageHP, persontageMP);
    }
}
