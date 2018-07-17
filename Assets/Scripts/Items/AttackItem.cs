//Created by Robert Bryant
//
//Creates an item that modifies the player's attack stats
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Item", menuName = "Items/Attack Item", order = 3)]
public class AttackItem : Items
{
    [Range(1, 10)]
    public float attack;            //Amount of attack the item changes
    [Range(1, 10)]
    public float defense;           //Amount of defense the item changes
    [Range(0.01f, 0.25f)]
    public float critChance;        //Amount of crit chance the item changes
    public bool onHit = false;      //Does the item have an ability on hit?
    [Range(0.01f, 0.2f)]
    public float hitChance = 0;     //Chance of the item activating

    //When an entity picks up the item
    public override void OnPickUp(Stats stats)
    {
        //Change the stats by a specifiecd amount
        stats.ChangeAttack(attack);
        stats.ChangeDefense(defense);
        stats.ChangeCritChance(critChance);
    }



}