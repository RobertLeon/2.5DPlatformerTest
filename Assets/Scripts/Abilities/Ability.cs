//Created by Robert Bryant
//Based off of Unity Tuttorial
//https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects
//Ability parent class
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName = "New Ability";          //Name of the ability
    public Sprite abilitySprite;                        //Sprite for the ability
    public float abilityCooldown;                       //Cooldown length for the ability
    [TextArea] public string abilityDescription;        //Ability Description

    //Initalize the components of the ability
    public abstract void Initialize(GameObject obj);

    //Activate the ability
    public abstract void TriggerAbility();

    //Describes the ability in the UI
    public abstract string DescribeAbility();
}