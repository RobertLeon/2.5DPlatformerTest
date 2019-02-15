//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/2DPlatformer-Tutorial
//Handles entity collision on different types of platforms

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionController : RaycastController
{
    public CollisionInfo collisions;        //Reference to CollisionInfo
    public float maxSlopeAngle = 80f;       //Maximum angle for moving on slopes

    [HideInInspector]
    public Vector2 playerInput;             //The player's input

    private Animator animator;              //Reference to the Animator component
    
    //Collision information for the player
    public struct CollisionInfo
    {
        public bool above, below;                       //Flags for which side is colliding 
        public bool left, right;                        //with an object
        public bool fallingThroughPlatform;             //Flag for falling through platforms        
        public bool climbingSlope, descendingSlope;     //Flags for climbing/descending slopes
        public bool slidingDownSlope;                   //Flag for slidining down a slope
        public bool canSlide;
        public bool sliding;                            //Flag for player activated slide
        public bool canClimb;                           //Flag for being able to climb an object
        public bool climbingObject;                     //Flag for currently climbing an object
        public bool inWater;
        public float slopeAngle, prevSlopeAngle;        //The angle of the slope
        public Vector2 prevMovement;                    //The player's previous movement amount
        public Vector2 slopeNormal;                     //Angle of the slope
        public float objSize;                           //Size of the object
        public int faceDir;                             //The direction the player is facing
                                                        //+1 is facing right -1 is facing left
                                                        
        //Reset the variables values
        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = descendingSlope = false;
            slidingDownSlope = false;
            slopeNormal = Vector2.zero;
            prevSlopeAngle = slopeAngle;
            slopeAngle = 0;
            objSize = 0;
        }

        //Reset the player's slide
        public void ResetSliding()
        {
            sliding = false;
        }
    }

    //Use this for initialization
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        //Set the player to facing right by default
        if (animator != null)
        {
            collisions.faceDir = 1;
            animator.SetFloat("FaceDir", collisions.faceDir);
        }
    }

    //Player movement
    public void Move(Vector2 movementAmount, Vector2 input, bool standingOnPlatform = false )
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.prevMovement = movementAmount;
        playerInput = input;

        //Descend Slops
        if (movementAmount.y < 0)
        {
            DescendSlope(ref movementAmount);
        }

        //Set the direction the player is facing
        if (movementAmount.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(movementAmount.x);
        }
        
        //Check player input for animation queues
        if(playerInput.x != 0 && !collisions.climbingObject)
        {
            animator.SetFloat("FaceDir", playerInput.x);
        }

        //Calculate horizontal collisions
        HorizontalCollision(ref movementAmount);

        //Calculate vertical collisions
        if (movementAmount.y != 0)
        {
            VerticalCollision(ref movementAmount);
        }
        
        //Move the player
        transform.Translate(movementAmount);

        //Allow jumping on moving platforms
        if(standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    //Overloaded Move
    public void Move(Vector2 movementAmount, bool standingOnPlatform)
    {
        Move(movementAmount, Vector2.zero, standingOnPlatform);
    }

    //Checks for vertical collisions
    private void VerticalCollision(ref Vector2 movementAmount)
    {
        //The sign of the direction we are moving in on the y-axis 
        //and the length of the rays for collision detection.
        float directionY = Mathf.Sign(movementAmount.y);
        float rayLength = Mathf.Abs(movementAmount.y) + skinWidth;

        //Loop through each vertical ray
        for(int i = 0; i < verticalRayCount; i++)
        {
            //Set the origin of the rays depending on the direction the player is moving.
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + movementAmount.x);

            //Drawing the vertical rays in scene view
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.white);

            //
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY,
                rayLength, collisionMask);

            //Check if the rays have hit an obstacle
            if (hit)
            {
                //Jump through the bottom of certain platforms
                if (hit.collider.tag == "Through")
                {
                    //If inside a platform keep falling
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }

                    //If falling through a platform keep falling
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }

                    //Drop down through certain platforms
                    if (playerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.25f);
                        continue;
                    }
                }

                movementAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
               

                //Check for climbing slopes
                if (collisions.climbingSlope)
                {
                    //Set the horizontal movement maount based on the vertical movement amount
                    //and slope angle
                    movementAmount.x =
                        movementAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad)
                        * Mathf.Sign(movementAmount.x);
                }
                //If directionY is -1 the player has collided with an object below them
                collisions.below = (directionY == -1);
                //If directionY is 1 the player has collided with an object above them
                collisions.above = (directionY == 1);
            }

        }

        //Smoothing out different slope climbs
        if(collisions.climbingSlope)
        {
            //Assign the sign of the movement on the x-axis and set the ray length.
            float directionX = Mathf.Sign(movementAmount.x);
            rayLength = Mathf.Abs(movementAmount.x) + skinWidth;

            //Set the origin of the rays depending on the direction the player is moving.
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft :
                raycastOrigins.bottomRight) + Vector2.up * movementAmount.y;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength,
                collisionMask);

            //Check for collisions with a different slope
            if(hit)
            {
                //The slope's angle
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                //If the slope is different from the collisions slope angle 
                if (slopeAngle != collisions.slopeAngle)
                {
                    movementAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    //Checks for horizontal collisions
    private void HorizontalCollision(ref Vector2 movementAmount)
    {
        //The sign of the direction the player is moving on the x-axis
        //and the length of the rays for collisions
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(movementAmount.x) + skinWidth;

        //Increase the rayLength based on movement
        if(Mathf.Abs(movementAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        //Draw rays in the scene view
        for (int i = 0; i < horizontalRayCount; i++)
        {
            //Set the origin of the rays depending on the direction the player is moving.
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            //Draw the horizontal rays
            Debug.DrawRay(rayOrigin, directionX * Vector2.right, Color.white);

            //
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX,
                rayLength, collisionMask);            

            //Check for collision
            if (hit)
            {
                //Size of the object being collided with
                collisions.objSize = hit.transform.localScale.y + hit.transform.localPosition.y;    
                
                //No collision when inside an object
                if (hit.distance == 0)
                {
                    continue;
                }

                //Get the angle of the surface we hit
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                //For the first iteration check if we are colliding with a slope
                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    //Reset the slope collision
                    if(collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        movementAmount = collisions.prevMovement;
                    }

                    float distanceToSlopeStart = 0;

                    //Check for changes in the slope's angle
                    if(slopeAngle != collisions.prevSlopeAngle)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        movementAmount.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref movementAmount, slopeAngle, hit.normal);
                    movementAmount.x += distanceToSlopeStart * directionX;
                }

                //Check for sliding down a slope
                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {                    
                    movementAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    //Adjust movemnt on the y-axis
                    if(collisions.climbingSlope)
                    {
                        movementAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad)
                            * Mathf.Abs(movementAmount.x);
                    }

                    //If directionX is -1 the player has collided with 
                    //an object to the left of them
                    collisions.left = (directionX == -1);

                    //If directionX is 1 the player has collided with
                    //an object to the right of them
                    collisions.right = (directionX == 1);
                }
            }
        }

    }

    //Climbing Slopes
    private void ClimbSlope(ref Vector2 movementAmount, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(movementAmount.x);
        float climbMovementY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        //Constant movment on the y-axis
        if (movementAmount.y <= climbMovementY)
        {
            movementAmount.y = climbMovementY;
            movementAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(movementAmount.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }
    }

    //Descending Slopes
    private void DescendSlope(ref Vector2 movementAmount)
    {
        //Maximum slope for sliding left
        RaycastHit2D maxSlopeLeft = Physics2D.Raycast(raycastOrigins.bottomLeft,
            Vector2.down, Mathf.Abs(movementAmount.y) + skinWidth, collisionMask);
        
        //Maximum slope for sliding right
        RaycastHit2D maxSlopeRight = Physics2D.Raycast(raycastOrigins.bottomRight,
            Vector2.down, Mathf.Abs(movementAmount.y) + skinWidth, collisionMask);

        //Check for sliding down slopes
        if (maxSlopeLeft ^ maxSlopeRight)
        {
            SlideDownMaxSlope(maxSlopeLeft, ref movementAmount);
            SlideDownMaxSlope(maxSlopeRight, ref movementAmount);
        }

        //If not sliding on a slope
        if (!collisions.slidingDownSlope)
        {
            float directionX = Mathf.Sign(movementAmount.x);

            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            //Did the raycasr hit anything
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                //Check for a slope
                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    //Check the direction of the slope
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        //Smooth the descent on the slope
                        if (hit.distance - skinWidth <= 
                            Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(movementAmount.x))
                        {
                            float moveDistance = Mathf.Abs(movementAmount.x);
                            float descentMovementY = Mathf.Sign(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            movementAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(movementAmount.x);
                            movementAmount.y -= descentMovementY;

                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.below = true;
                            collisions.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    //Resets the check for falling through platforms
    private void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    //Sliding down slopes
    void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 movementAmount)
    {
        //Check for slope side
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            //If the slope angle is greater than the maximum slide down the slope
            if (slopeAngle > maxSlopeAngle)
            {
                movementAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(movementAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.slopeAngle = slopeAngle;
                collisions.slidingDownSlope = true;
                collisions.slopeNormal = hit.normal;
            }
        }
    }
}