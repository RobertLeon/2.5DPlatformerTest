//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/2DPlatformer-Tutorial
//Handles the movement of the player
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
    public float slide;                             //Force for sliding on the floor
    public float slideTime = 0.4f;
    [HideInInspector]
    public bool wallSliding;                        //Check if the player is sliding on a wall         
    [HideInInspector]
    public bool isCrouching;                        //Check if the player is crouching
    [HideInInspector]
    public bool isJumping = false;                  //Check for the player jumping
    [HideInInspector]
    public Vector2 directionalInput;                //Input for the players movement
    

    private PlayerStats playerStats;                //Reference to the Player Stats script
    private CollisionController collision;          //Reference to the CollisionController
    private float gravity;                          //Downward force on the player
    private float minJumpVelocity;                  //Minimum jump velocity
    private float maxJumpVelocity;                  //Maximum jump velocity
    private Vector2 velocity;                       //Velocity of the player movement
    private float velocityXSmoothing;               //Smoothing movement on the x-axis
    private float accelTimeAir = .2f;               //Time to reach maximum velocity in the air 
    private float accelTimeGround = .1f;            //Time to reach maximum velocity on the ground
    private float timeToWallUnstick;                //Counter till the player unsticks from the wall
    private float movementSpeed;                    //
    private float jumpApex;                         //
    private int wallDirX;                           //Direction of the wall on the x-axis
    private int jumps;                              //Jump counter
    private bool updateMovement = false;            //Check to recalculate movement variables

    
    private void OnEnable()
    {
        PlayerInput.JumpButtonDown += OnJumpInputDown;
        PlayerInput.JumpButtonUp += OnJumpInputUp;
        PlayerInput.MovementInput += SetDirectionalInput;
    }

    private void OnDisable()
    {
        PlayerInput.JumpButtonDown -= OnJumpInputDown;
        PlayerInput.JumpButtonUp -= OnJumpInputUp;
        PlayerInput.MovementInput -= SetDirectionalInput;
    }
    
    //Use this for initialization
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        collision = GetComponent<CollisionController>();
        jumps = playerStats.movement.numJumps;
        jumpApex = timeToJumpApex;
        ChangeMovementSpeed(0);
        CalculateMovement();
    }

    //Update is called once per frame
    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();
        HandleClimbing();

        if (collision.collisions.below && directionalInput.y == -1)
        {
            isCrouching = true;

            if (!collision.collisions.sliding)
            {
                velocity.x = 0;
            }
        }
        else
        {
            isCrouching = false;
        }

        //Move the player
        collision.Move(velocity * Time.deltaTime, directionalInput);

        //If the player collides with something above or below them
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

        

        //Reset number of jumps
        if (collision.collisions.below || 
            collision.collisions.climbingObject ||
            wallSliding)
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
    
    //Update the movement variables of the player
    public void UpdateMovement()
    {
        updateMovement = true;
    }

    
    //Increase the amount of jumps the player can perform
    public void IncreaseJumpCount()
    {
        jumps++;
    }

    public void ChangeMovementSpeed(float amount)
    {
        movementSpeed = moveSpeed + amount;
    }
       
    //Calculates the player's gravity
    private void CalculateMovement()
    {
        gravity = -(2 * (maxJumpHeight + playerStats.movement.jumpHeightModifier)) / Mathf.Pow(timeToJumpApex, 2);

        maxJumpVelocity = Mathf.Abs(gravity) * jumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    //Set the player's input
    private void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;        
    }

    //Handle the player jumping
    private void OnJumpInputDown()
    { 
        //If the player can jump in the air
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
            }
            //Jumping off the wall
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;                
            }
            //Leaping from wall to wall
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;                
            }
            isJumping = true;
            jumps--;
        }

        //The player is on the ground
        if (collision.collisions.below)
        {
            //If sliding down a slope
            if (collision.collisions.slidingDownSlope)
            {
                //Not jumping against the slope
                if (directionalInput.x != -Mathf.Sign(collision.collisions.slopeNormal.x))
                {
                    velocity.y = maxJumpVelocity * collision.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * collision.collisions.slopeNormal.x;
                    jumps--;
                    isJumping = true;
                }
            }
            //Not sliding down a slope
            else
            {
                //Perform a slide
                if (directionalInput.y == -1 && !collision.collisions.sliding)
                {
                    collision.collisions.sliding = true;
                    velocity.x = slide * collision.collisions.faceDir;
                    Invoke("ResetSliding", slideTime);
                }
                //If the player has jump available
                else if (jumps > 0 && !collision.collisions.sliding)
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
    
    //Resets the ability to slide
    private void ResetSliding()
    {
        collision.collisions.ResetSliding();
    }
    
    //Variable jump height
    private void OnJumpInputUp()
    {
        //If the player is moving faster than the minimum jump velocity
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    //Calculate the player's moving velocity
    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * (movementSpeed);

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (collision.collisions.below) ? accelTimeGround : accelTimeAir);

        velocity.y += gravity * Time.deltaTime;

        if (velocity.y <= gravity / 2.5f)
        {
            velocity.y = gravity / 2.5f;
        }
       
    }

    //Handle the player's movement for climbing up and down objects
    private void HandleClimbing()
    { 
        //Stop the player from climbing down at the bottom of the ladder and allows to climb up
        if (!collision.collisions.below || (collision.collisions.below && directionalInput.y >= 0.65f))
        {
            //If the player can climb up or down
            if (collision.collisions.canClimb && directionalInput.y >= 0.65f ||
                collision.collisions.canClimb && directionalInput.y <= -0.65f)
            {
                //Check if the player has not activated a slide
                if (!collision.collisions.sliding)
                {
                    velocity.y = Mathf.Sign(directionalInput.y) * (climbSpeed + playerStats.movement.speedModifier);

                    velocity.x = 0f;

                    collision.collisions.climbingObject = true;
                }
            }

            //Stop climbing when on the ground
            if (collision.collisions.below)
            {
                collision.collisions.climbingObject = false;
            }

            //If the player is climbing an object and has stopped
            if (collision.collisions.climbingObject && directionalInput.y == 0)
            {
                velocity.y = 0f;

                velocity.x = 0f;
            }
        }
    }

    //Handle the player's movement while sliding on a wall
    private void HandleWallSliding()
    {
        //Direction of a wall in relation to the player
        wallDirX = (collision.collisions.left) ? -1 : 1;

        wallSliding = false;

        //Wall Sliding
        if ((collision.collisions.left || collision.collisions.right)
            && !collision.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            //Constant wall slide speed
            if(velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            //Allows the player to stick to the wall for better jumping control
            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;
                
                //Count down as long as the player is not moving into the wall
                if (directionalInput.x != wallDirX)
                {
                    timeToWallUnstick -= Time.deltaTime;                    
                }
                //Reset the timer
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            //Reset the timer
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    //
    public Vector2 GetVelocity()
    {
        return velocity;
    }
}