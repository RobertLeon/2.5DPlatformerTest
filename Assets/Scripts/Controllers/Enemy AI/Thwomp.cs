//Created by Robert Bryant
//
//Handles the movement of falling enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    public Vector2 dropSpeed;                   //Speed the obstacle drops
    public Vector2 riseSpeed;                   //Speed the obstacle rises
    public float waitTime = 1.5f;              //Time the obstacle waits until it rises

    private CollisionController collision;      //Reference to the Collision Controller Script
    private bool drop = false;                  //Is the obstacle falling?
    private bool rise = false;                  //Is the obstacle rising?
    private bool reset = true;                  //Has the obstacle returned to the starting position?
    private float riseTimer;                    //Counter for the rise timer
    

	//Use this for initialization
	void Start()
	{
        collision = transform.parent.GetComponent<CollisionController>();
	}

    //Update is called once per frame
    void Update()
    {
        //If the obstacle is dropping down
        if (drop)
        {
            collision.Move(dropSpeed * Time.deltaTime, Vector2.zero);
        }

        //If the obstacle is rising up
        if (rise)
        {
            if (riseTimer < 0)
            {
                collision.Move(riseSpeed * Time.deltaTime, Vector2.zero);
            }
            riseTimer -= Time.deltaTime;
        }

        //Check if the obstacle has collided with something below it
        if(collision.collisions.below)
        {            
            rise = true;
            drop = false;            
        }

        //Check if the obstacle has collided with something above it
        if(collision.collisions.above)
        {
            drop = false;
            rise = false;
            reset = true;
        }
	}


    //When an object is in an area
    private void OnTriggerStay(Collider other)
    {
        //Checks if the player has enetered the area and the obstacle is reset
        if(other.tag == "Player" && reset)
        {
            drop = true;
            reset = false;
            riseTimer = waitTime;
        }
    }
}