//Created by Robert Bryant
//
//Handles the movement of ground enemies

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyController : EnemyMovement
{

    public MovementType movementType;
    //private Vector3 startPos;

    public override void Start()
    {
        base.Start();

        //startPos = transform.position;

        if (isFlying)
            canJump = false;
        
    }


    //Use this for initialization
    void Update()
    {
        CalculateVelocity();

        collision.Move(velocity * Time.deltaTime, movementDir);

        //If the enemy collides with something above or below them
        if (collision.collisions.above || collision.collisions.below)
        {
            //Check for sliding down a slope
            if (collision.collisions.slidingDownSlope)
            {
                velocity.y += collision.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
               velocity.y = 0;
            }
        }

        //Reset the ability to jump when the enemy hits the ground
        if (collision.collisions.below)
        {
            hasJumped = false;
        }

        //Move the enemy based on the movement type
        switch (movementType)
        {
            //No movement at all
            case MovementType.None:
                velocity.x = 0;
                movementDir.x = 0;
                break;

            //The enemy will pace back and forth on platforms and
            //will not fall off unless pushed
            case MovementType.Pace:

                //The enemy will move to the right if there is no movement direction
                if (movementDir.x == 0)
                {
                    movementDir.x = 1;
                }

                //If the enemy can jump and has collided with an object it can jump over
                //then attempt to jump over it
                if (canJump && !hasJumped && (maxObstacle >= collision.collisions.objSize) &&
                    (collision.collisions.left || collision.collisions.right))
                {
                   hasJumped = true;
                    velocity.y = maxJumpVelocity;
                }
                //Otherwise stop and reverse direction
                else if (!hasJumped &&
                    (collision.collisions.left || collision.collisions.right))
                {
                    velocity.x = 0;
                    movementDir.x *= -1;
                }

                //Stop the enemy from falling off a ledge
                if (collision.collisions.below == false && !hasJumped)
                {
                   velocity.x *= -1;
                    movementDir.x *= -1;
                }
                break;


            //The enemy will just keep walking until it collides with something
            case MovementType.Walk:

                //The enemy will move to the right if there is no movement direction
                if (movementDir.x == 0)
                {
                    movementDir.x = 1;
                }

                //If the enemy can jump and has collided with an object it can jump over
                //then attempt to jump over it
                if (canJump && !hasJumped && (maxObstacle >= collision.collisions.objSize) &&
                    (collision.collisions.left || collision.collisions.right))
                {
                    hasJumped = true;
                    velocity.y = maxJumpVelocity;
                }
                //Otherwise stop and reverse direction
                else if (!hasJumped &&
                    (collision.collisions.left || collision.collisions.right))
                {
                    velocity.x = 0;
                    movementDir.x *= -1;
                }
                break;

            case MovementType.Hop:

                //The enemy will move to the right if there is no movement direction
                if (movementDir.x == 0)
                {
                   movementDir.x = 1;
                }

                //Make the enemy jump when it lands on the ground
                if (canJump && !hasJumped)
                {
                    velocity.y = maxJumpVelocity;
                    hasJumped = true;
                }

                //Reverse direction when they collide with a wall
                if (collision.collisions.left || collision.collisions.right)
                {
                   velocity.x = 0;
                   movementDir *= -1;
                }

                break;

            default:
                throw new System.Exception("Enemy AI type not assigned on: " + transform.name);
        }
    }
}

