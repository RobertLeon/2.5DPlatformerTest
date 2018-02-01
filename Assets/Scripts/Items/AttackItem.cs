using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Item", menuName = "Items/Attack Item", order = 3)]
public class AttackItem : Items
{
    public float attack;
    public float defense;
    public float critChance;
    public float attackSpeed;

    public override void Initialize(PlayerStats playerStats)
    {
        playerStats.ChangeAttack(attack);
        playerStats.ChangeAttackSpeed(attackSpeed);
        playerStats.ChangeDefense(defense);
        playerStats.ChangeCritChance(critChance);
    }
}