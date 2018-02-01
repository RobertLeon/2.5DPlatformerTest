using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abilities : ScriptableObject
{
    public string abilityName = "New Ability";
    public Sprite abilitySprite;
    public float abilityCooldown;


    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();
}