using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemAddHealth : MonoBehaviour, IInteractable
{
    public int addHealth;
    public GameObject itemAddHealth;
    public void TriggerAction(Character character)
    {
         TakeItem(character);
    }

    public void TakeItem(Character character)
    {
        character.maxHealth += addHealth;
        character.currentHealth += addHealth;
        character.OnHealthChange?.Invoke(character);
        itemAddHealth.SetActive(false);
    }
}
