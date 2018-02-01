using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;                    //Player movement speed
    public float climbSpeed = 6f;                   //Player climb speed
    public float minJumpHeight = 1f;                //Minimum jump height
    public float maxJumpHeight = 4f;                //Maximum jump height
    public float timeToJumpApex = 0.4f;             //Time needed to reach the apex of a jump
    public float wallSlideSpeedMax = 3f;            //Speed for sliding down a wall
    public float wallStickTime = 0.5f;              //Time to stick to a wall
    public Vector2 wallJumpClimb;                   //Force for a climbing wall jump
    public Vector2 wallJumpOff;                     //Force for jumping off a wall
    public Vector2 wallLeap;                        //Force for leaping between walls      

    private PlayerStats playerStats;
    private CollisionController collision;          //Reference to the CollisionController
    private float gravity;                          //Downward force on the player
    private float minJumpVelocity;                  //Minimum jump velocity
    private float maxJumpVelocity;                  //Maximum jump velocity
    private Vector2 velocity;                       //Velocity of the player movement
    private float velocityXSmoothing;               //Smoothing movement on the x-axis
    private float accelTimeAir = .2f;               //Time to reach maximum velocity in the air 
    private float accelTimeGround = .1f;            //Time to reach maximum velocity on the ground
    private float timeToWallUnstick;                //Counter till 
    private Vector2 directionalInput;               //Input
    private bool wallSliding;                       //Is the player sliding on a wall?
    private int wallDirX;                           //Direction of the wall on the x-axis
    private int jumps;                              //Jump counter
    private bool isJumping = false;                 //Is the player jumping?
    private bool updateMovement = false;            //Check to recalculate movement variables

    //Use this for initialization
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        collision = GetComponent<CollisionController>();
        jumps = playerStats.movement.numJumps;
        CalculateMovement();
    }

    //Update is called once per frame
    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();
        HandleClimbing();

        collision.Move(velocity * Time.deltaTime, directionalInput);


        //If the player collides with something above or below them
        if (collision.collisions.above || collision.collisions.below)
        {
            if (collision.collisions.slidingDownSlope)
            {
                velocity.y += collision.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }

        //Reset number of jumps
        if (collision.collisions.below || collision.collisions.climbingObject || wallSliding)
        {
            jumps = playerStats.movement.numJumps;
            isJumping = false;
        }

        //Update Gravity
        if (updateMovement)
        {
            CalculateMovement();
            updateMovement = false;
        }

    }

    //
    public void UpdateMovement()
    {
        updateMovement = true;
    }

    //
    public void IncreaseJumpCount()
    {
        jumps++;
    }

    //Calculates the player's gravity
    private void CalculateMovement()
    {
        moveSpeed += playerStats.movement.speedModifier;
        gravity = -(2 * (maxJumpHeight + playerStats.movement.jumpHeightModifier)) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    //Set the player's input
    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;        
    }

    //
    public void OnJumpInputDown()
    { 
        if (isJumping && jumps > 0)
        {
            velocity.y = maxJumpVelocity * 0.75f;
            jumps--;
        }

        //Wall Jumps
        if (wallSliding)
        {
            //Climbing up the wall
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
                jumps--;
                isJumping = true;
            }
            //Jumping off the wall
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
                jumps--;
                isJumping = true;
            }
            //Leaping from wall to wall
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
                jumps--;
                isJumping = true;
            }
        }

        //The player is on the ground
        if (collision.collisions.below)
        {
           if(collision.collisions.slidingDownSlope)
            {
                //Not jumping against the slope
                if(directionalInput.x != -Mathf.Sign(collision.collisions.slopeNormal.x))
                {
                    velocity.y = maxJumpVelocity * collision.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * collision.collisions.slopeNormal.x;
                    jumps--;
                    isJumping = true;
                }
            }
            else
            {
                if (jumps > 0)
                {
                    velocity.y = maxJumpVelocity;
                    jumps--;
                    isJumping = true;
                }
            }            
        }
        
        //Jump off of a ladder
        if(collision.collisions.climbingObject)
        {
            velocity.x = wallJumpOff.x * Mathf.Sign(directionalInput.x);
            velocity.y = wallJumpOff.y;
            collision.collisions.climbingObject = false;
        }
    }
    
    //Variable jump height
    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
    }

    //Calculate the player's moving velocity
    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * (moveSpeed * playerStats.movement.speedModifier);

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (collision.collisions.below) ? accelTimeGround : accelTimeAir);

        velocity.y += gravity * Time.deltaTime;
    }

    //
    private void HandleClimbing()
    {
        //
        if (collision.collisions.canClimb && directionalInput.y == 1 || 
            collision.collisions.canClimb && directionalInput.y == -1)
        {
            velocity.y = Mathf.Sign(directionalInput.y) * climbSpeed * playerStats.movement.speedModifier;
            velocity.x = 0;
            collision.collisions.climbingObject = true;
        }

        //
        if(collision.collisions.climbingObject && directionalInput.y == 0)
        {
            velocity.y = 0;
            velocity.x = 0;                      
        }

        //
        if(collision.collisions.climbingObject && collision.collisions.below)
        {
            collision.collisions.climbingObject = false;
        }
    }

    //
    private void HandleWallSliding()
    {
        //Direction of a wall in relation to the player
        wallDirX = (collision.collisions.left) ? -1 : 1;

        wallSliding = false;

        //Wall Sliding
        if ((collision.collisions.left || collision.collisions.right)
            && !collision.collisions.below && velocity.y < 0 )
        {
            wallSliding = true;

            if(velocity.y < wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x == 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }
}