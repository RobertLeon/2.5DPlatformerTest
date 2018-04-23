//Created by Robert Bryant
//
//Handles the movement of most enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float minJumpHeight = 1f;
    public float maxJumpHeight = 4f;
    public float timeToJumpApex = 0.4f;
    public bool isFlying = false;
    public bool canJump = false;
    [HideInInspector]
    public Vector2 velocity;
    [HideInInspector]
    public float gravity;
    [HideInInspector]
    public float maxJumpVelocity;
    [HideInInspector]
    public Vector2 movementDir;
    [HideInInspector]
    public bool hasJumped;
    [HideInInspector]
    public float maxObstacle;
    [HideInInspector]
    public CollisionController collision;

    private EnemyStats enemyStats;
    private float velocityXSmoothing;         //Smoothing movement on the x-axis
    private float accelTimeAir = 0.1f;               //Time to reach maximum velocity in the air 
    private float accelTimeGround= 0.2f;            //Time to reach maximum velocity on the ground

    //private float minJumpVelocity;
    
    private bool updateMovement;


    public virtual void Start()
    {
        collision = GetComponent<CollisionController>();
        enemyStats = GetComponent<EnemyStats>();
        maxObstacle = maxJumpHeight + transform.localScale.y;
        CalculateMovement();
    }


    //Calculate the enemy's movement velocity
    public void CalculateVelocity()
    {
        float targetVelocityX = movementDir.x * (moveSpeed);

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
        moveSpeed += enemyStats.movement.speedModifier;

        if (!isFlying)
        {
            gravity = -(2 * (maxJumpHeight + enemyStats.movement.jumpHeightModifier))
                / Mathf.Pow(timeToJumpApex, 2);
        }
        else
        {
            gravity = 0;
        }

        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }
}

public enum MovementType
{
    None,
    Walk,
    Pace,
    Hop
}
