using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAddMP : MonoBehaviour, IInteractable
{
    public int addMP;
    public void TriggerAction(Character character)
    {
        TakeItem(character);
    }

    public void TakeItem(Character character)
    {
        character.maxMP += addMP;
        character.currentMP += addMP;
        character.OnHealthChange?.Invoke(character);
        gameObject.SetActive(false);
    }
}