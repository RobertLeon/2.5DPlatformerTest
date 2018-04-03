//Created by Robert Bryant
//
//Creates an item that modifies the player's attack stats

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Item", menuName = "Items/Attack Item", order = 3)]
public class AttackItem : Items
{
    [Range(1,10)]
    public float attack;            //Amount of attack the item changes
    [Range(1, 10)]
    public float defense;           //Amount of defense the item changes
    [Range(0.01f, 0.25f)]
    public float critChance;        //Amount of crit chance the item changes
    [Range(0.01f, 0.25f)]
    public float attackSpeed;       //Amount of attack speed the item changes

    public override void Initialize(PlayerStats playerStats)
    {
        //Change the stats by a specifiecd amount
        playerStats.ChangeAttack(attack);
        playerStats.ChangeAttackSpeed(-attackSpeed);
        playerStats.ChangeDefense(defense);
        playerStats.ChangeCritChance(critChance);
    }
}