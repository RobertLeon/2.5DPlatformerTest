//Created by Robert Bryany
//
//Creates an item that modifies the player's movement stats

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Movement Item", menuName = "Items/Movement Item", order = 3)]
public class MovementItem : Items
{
    public bool increaseJumps = false;      //Increase the amount of jumps
    [Range(0.1f, 1.0f)]
    public float jumpHeight;                //Amount to increase the jump height
    [Range(0.1f, 1.0f)]
    public float moveSpeed;                 //Amount to increase the movement speed


    //Initialize the item
    public override void OnPickUp(Stats stats)
    {
        //Check if the item increases jump amount
        if (increaseJumps)
        {
            stats.IncreaseJumps();
        }

        //Increase jump height and movement speed by the specified amounts
        stats.IncreaseJumpHeight(jumpHeight);
        stats.IncreaseSpeed(moveSpeed);
    }

}