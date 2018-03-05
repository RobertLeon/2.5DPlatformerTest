using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Movement Item", menuName = "Items/Movement Item", order = 2)]
public class MovementItem : Items
{
    public bool increaseJumps = false;      //Increase the amount of jumps
    [Range(0.1f, 1.0f)]
    public float jumpHeight;                //Amount to increase the jump height
    [Range(0.1f, 1.0f)]
    public float moveSpeed;                 //Amount to increase the movement speed


    //Initialize the item
    public override void Initialize(PlayerStats playerStats)
    {
        //Check if the item increases jump amount
        if (increaseJumps)
        {
            playerStats.IncreaseJumps();
        }

        //Increase jump height and movement speed by the specified amounts
        playerStats.IncreaseJumpHeight(jumpHeight);
        playerStats.IncreaseSpeed(moveSpeed);
    }
}