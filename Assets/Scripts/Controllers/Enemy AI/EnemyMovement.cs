//Created by Robert Bryant
//
//Handles the calculation of the enemy movement
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 6f;                    //Movement Speed
    public float minJumpHeight = 1f;                //Minimum Jump height
    public float maxJumpHeight = 4f;                //Maximum jump height
    public float timeToJumpApex = 0.4f;             //Time to reach the height of the jump
    public bool canJump = false;                    //Can this enemy jump?
    [HideInInspector]
    public Vector2 velocity;                        //Enemey's movement velocity
    [HideInInspector]
    public float gravity;                           //Enemy's gravity
    [HideInInspector]
    public float maxJumpVelocity;                   //Maximum jump velocity
    [HideInInspector]
    public Vector2 movementDir;                     //Direction the enemy is moving
    [HideInInspector]
    public bool hasJumped;                          //Has this enemy jumped
    [HideInInspector]
    public float maxObstacle;                       //Maximum height of an obsticle that can be jumped over
    [HideInInspector]
    public CollisionController collision;

    private EnemyStats enemyStats;                  //Reference to the enemy stats script
    private float velocityXSmoothing;               //Smoothing movement on the x-axis
    private float movementSpeed;
    private float accelTimeAir = 0.1f;              //Time to reach maximum velocity in the air 
    private float accelTimeGround = 0.2f;           //Time to reach maximum velocity on the ground
    private bool updateMovement;                    //Check for updating enemy movement

    //Use this for initialization
    public virtual void Start()
    {
        //Get the collision controller and enemy stat scripts
        collision = GetComponent<CollisionController>();
        enemyStats = GetComponent<EnemyStats>();
        
        //Set the maximum height of an obstacle an enemy can traverse 
        maxObstacle = maxJumpHeight + transform.localScale.y;
       
        //Calculate the enemy's movement
        CalculateMovement();
    }


    //Calculate the enemy's movement velocity
    public void CalculateVelocity()
    {
        float targetVelocityX = movementDir.x * movementSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (collision.collisions.below) ? accelTimeGround : accelTimeAir);

        velocity.y += gravity * Time.deltaTime;
    }

    //
    private void Update()
    {
        if(updateMovement)
        {
            CalculateMovement();
            updateMovement = false;
        }
    }

    //Update the movement variables of the enemy
    public void UpdateMovement()
    {
        updateMovement = true;
    }


    //Calculates the enemy's gravity and movement speed
    public void CalculateMovement()
    {
        movementSpeed = moveSpeed + enemyStats.movement.speedModifier;

        gravity = -(2 * (maxJumpHeight + enemyStats.movement.jumpHeightModifier))
                / Mathf.Pow(timeToJumpApex, 2);        
       
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;       
    }
}

public enum MovementType
{
    None,
    Walk,
    Pace,
    Hop
}
