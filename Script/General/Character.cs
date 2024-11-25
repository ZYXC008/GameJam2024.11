using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO NewGameEvent;
    [Header("基础属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxMP;
    public float currentMP;
    public float shield;
    public float runOffHealth;

    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;
    private void OnEnable()
    {
        NewGameEvent.OnEventRiased += NewGame;
        currentHealth = maxHealth;
    }

    private void OnDisable()
    {
        NewGameEvent.OnEventRiased -= NewGame;
    }
    
    private void NewGame()
    {
        currentHealth = maxHealth;
        currentMP = maxMP;
        OnHealthChange?.Invoke(this);
    }
    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;
        if(currentHealth + shield - attacker.damage > 0)
        {
            if(shield - attacker.damage >= 0)
            {
                shield -= attacker.damage;
            }
            else
            { 
                currentHealth -= (attacker.damage - shield);
                shield = 0;
            }
            
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
        }
    }
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
    public void FieldOpen(bool isField)
    {
        if (isField)
            currentHealth -= runOffHealth;
    }
}
