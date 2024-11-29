using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, ISaveable
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
    public float invulnerableCounter;
    public bool invulnerable;

    [Header("定身")]
    public float stopDuration;
    public float stopCounter;
    public bool stop;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;
    private void OnEnable()
    {
        NewGameEvent.OnEventRiased += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
        currentHealth = maxHealth;
    }

    private void OnDisable()
    {
        NewGameEvent.OnEventRiased -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
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
        if(stop)
        {
            stopCounter -= Time.deltaTime;
            if (stopCounter <= 0)
            {
                stop = false;
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
        else
        {
            currentHealth = 0;
            //触发死亡
            OnDead?.Invoke();
        }
        OnHealthChange?.Invoke(this);
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
        {
            currentHealth -= runOffHealth;
            OnHealthChange?.Invoke(this);
        }
           
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = transform.position;
            data.floatSavedData[GetDataID().ID + "health"] = currentHealth;
            data.floatSavedData[GetDataID().ID + "MP"] = currentMP;
            data.floatSavedData[GetDataID().ID + "shield"] = shield;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            data.floatSavedData.Add(GetDataID().ID + "health",this.currentHealth);
            data.floatSavedData.Add(GetDataID().ID + "shield", this.shield);
            data.floatSavedData.Add(GetDataID().ID + "MP", this.currentMP);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            this.transform.position = data.characterPosDict[GetDataID().ID];
            this.currentHealth = data.floatSavedData[GetDataID().ID + "health"];
            this.currentMP = data.floatSavedData[GetDataID().ID + "MP"];
            this.shield = data.floatSavedData[GetDataID().ID + "shield"];
        }
    }
}
