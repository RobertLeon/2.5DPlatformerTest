using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionController))]
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float minJumpHeight = 1f;
    public float maxJumpHeight = 4f;
    public float timeToJumpApex = 0.4f;

    private CollisionController collision;
    private Vector2 velocity;
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;



    //Use this for initialization
    void Start()
	{
        collision = GetComponent<CollisionController>();

        CalculateGravity();
	}

	//Update is called once per frame
	void Update()
	{
        CalculateVelocity();

        //If the enemy collides with something above or below them
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

        collision.Move(velocity,Vector2.zero);

    }

    private void EnemyJump()
    {
        if(Random.Range(0f,1f) > 0.5f)
        {
            velocity.y = maxJumpVelocity;
        }
        else
        {
            velocity.y = minJumpVelocity;
        }
    }

    //Calculate the enemy's moving velocity
    private void CalculateVelocity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    //Calculates the enemy's gravity
    private void CalculateGravity()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }
}