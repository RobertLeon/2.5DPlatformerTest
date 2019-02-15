//Created by Robert Bryant
//
//Handles the movement of ground enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovementController : EnemyMovement
{
    public MovementType movementType;               //Type of movement the enemy uses

    private Vector3 startPos;                       //Starting position of the enemy
    private bool turnAround = false;                //Trigger for turning an enmy around

    //Use this for initialization
    public override void Start()
    {
        base.Start();

        startPos = transform.position;
    }


    //
    void Update()
    {
        CalculateVelocity();

        collision.Move(velocity * Time.deltaTime, movementDir);

        EnemeyMove();
    }

    //Turns the pacing enemy around
    private IEnumerator TurnAround()
    {        
        //Set the movement of the enemy to the oppsite direction
        velocity.x *= -1;
        movementDir *= -1;

        //Wait half a second to reset the trigger to turn around
        yield return new WaitForSeconds(0.5f);
        turnAround = false;
    }

    //Moves the enemy
    private void EnemeyMove()
    {
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

                //If the enemy is not turning around
                if (!turnAround)
                {
                    List<bool> onPlatform = new List<bool>();
                    float rayLength = 0.025f;

                    //Loop through each vertical ray
                    for (int i = 0; i < collision.verticalRayCount; i++)
                    {
                        Vector2 rayOrigin = collision.raycastOrigins.bottomLeft + Vector2.right * (collision.verticalRaySpacing * i);

                        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, collision.collisionMask);

                        //Add the result from the raycast to the list
                        onPlatform.Add(hit);
                    }

                    //Check if any of the rays are not hitting a platform
                    if (onPlatform.Contains(false))
                    {
                        //Start the process for turning the enemy around
                        turnAround = true;
                        StartCoroutine(TurnAround());
                    }
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

            //Hopping Enemy
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

            //Error Message
            default:
                throw new System.Exception("Enemy AI type not assigned on: " + transform.name);
        }
        
    }
}
