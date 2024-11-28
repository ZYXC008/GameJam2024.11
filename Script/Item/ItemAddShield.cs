using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAddShield : MonoBehaviour, IInteractable
{
    public int addShield;
    public void TriggerAction(Character character)
    {
        TakeItem(character);
    }

    public void TakeItem(Character character)
    {
        character.shield += addShield;
        character.OnHealthChange?.Invoke(character);
        gameObject.SetActive(false);
    }
}