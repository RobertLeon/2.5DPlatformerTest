using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Movement Item", menuName = "Items/Movement Item", order = 2)]
public class MovementItem : Items
{
    public bool increaseJumps = false;
    public float jumpHeight;
    public float moveSpeed;


    public override void Initialize(PlayerStats playerStats)
    {
        if (increaseJumps)
        {
            playerStats.IncreaseJumps();
        }

        playerStats.IncreaseJumpHeight(jumpHeight);
        playerStats.IncreaseSpeed(moveSpeed);
    }
}