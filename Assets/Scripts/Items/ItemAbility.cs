//Created by Robert Bryany
//
//Item effect activation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbility : ScriptableObject
{
    //Type of trigger for an item's special ability
    public enum ItemActivator
    {
        OnHit,
        OnDamageTaken,
        OnKill,
        OnDeath,
        OnCrit,
    }

    public ItemActivator trigger;               //Item trigger type
    [Range(0.0f, 1.0f)]
    public float activationChance;              //Activation chance of the item
    public GameObject itemEffect;               //Effect the item has
    public float coolDown;                      //Cool down on the item effect
}
