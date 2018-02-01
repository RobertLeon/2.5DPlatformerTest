﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionController : RaycastController
{
    public CollisionInfo collisions;        //Reference to CollisionInfo
    public float maxSlopeAngle = 80f;       //Maximum angle for moving on slopes

    [HideInInspector]
    public Vector2 playerInput;             //The player's input
    
    //Collision information for the player
    public struct CollisionInfo
    {
        public bool above, below;                       //Flags for which side is colliding 
        public bool left, right;                        //with an object
        public bool fallingThroughPlatform;             //Flag for falling through platforms        
        public bool climbingSlope, descendingSlope;     //Flags for climbing/descending slopes
        public bool slidingDownSlope;
        public bool canClimb;
        public bool climbingObject;
        public float slopeAngle, prevSlopeAngle;        //The angle of the slope
        public Vector2 prevMovement;                    //The player's previous movement amount
        public Vector2 slopeNormal;
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
        }
    }

    //
    public override void Start()
    {
        base.Start();

        //Set the player to facing right by default
        collisions.faceDir = 1;
    }

    //Player movement
    public void Move(Vector2 movementAmount, Vector2 input, bool standingOnPlatform = false)
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

        //Draw rays in the scene view
        for(int i = 0; i < verticalRayCount; i++)
        {
            //Set the origin of the rays depending on the direction the player is moving.
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + movementAmount.x);

            //Drawing the vertical rays in scene view
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.white);

            //
            RaycastHit hit;

            //Check if the rays have hit an obstacle
            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, rayLength, collisionMask))
            {
                //Jump through the bottom of certain platforms
                if (hit.collider.tag == "Through" || hit.collider.tag == "Climbable")
                {
                    
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.5f);
                        continue;
                    }
                }
                
                //
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

            RaycastHit hit;

            //Check for collisions with a different slope
            if(Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, rayLength, collisionMask))
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
            RaycastHit hit;            

            //Check for collision
            if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, rayLength, collisionMask))
            {
                //No collision with climbable objects
                if(hit.collider.tag == "Climbable")
                {
                    if(directionX == 1 || directionX == -1)
                    {
                        continue;
                    }
                }

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
                    if(collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        movementAmount = collisions.prevMovement;
                    }

                    float distanceToSlopeStart = 0;

                    if(slopeAngle != collisions.prevSlopeAngle)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        movementAmount.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref movementAmount, slopeAngle, hit.normal);
                    movementAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    movementAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

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
        RaycastHit hit;
        bool maxSlopeLeft = Physics.Raycast(raycastOrigins.bottomLeft, Vector2.down, out hit, Mathf.Abs(movementAmount.y) + skinWidth, collisionMask);
        bool maxSlopeRight = Physics.Raycast(raycastOrigins.bottomRight, Vector2.down, out hit, Mathf.Abs(movementAmount.y) + skinWidth, collisionMask);

        if (maxSlopeLeft ^ maxSlopeRight)
        {
            SlideDownMaxSlope(maxSlopeLeft, hit, ref movementAmount);
            SlideDownMaxSlope(maxSlopeRight, hit, ref movementAmount);
        }

        if (!collisions.slidingDownSlope)
        {
            float directionX = Mathf.Sign(movementAmount.x);

            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            
            if (Physics.Raycast(rayOrigin, -Vector2.up, out hit, Mathf.Infinity, collisionMask))
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(movementAmount.x))
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

    //
    private void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    //
    void SlideDownMaxSlope(bool side, RaycastHit hit, ref Vector2 movementAmount)
    {
        if (side)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

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