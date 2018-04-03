//Created by Robert Bryant
//
//Handles the movement of falling enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    public Vector2 dropSpeed;
    public Vector2 riseSpeed;

    private CollisionController collision;
    private bool drop = false;
    private bool rise = false;
    private bool reset = true;
    private float riseTimer;
    private float waitTime = 1.5f;

	//Use this for initialization
	void Start()
	{
        collision = transform.parent.GetComponent<CollisionController>();
	}

    //Update is called once per frame
    void Update()
    {
        if (drop)
        {
            collision.Move(dropSpeed * Time.deltaTime, Vector2.zero);
        }

        if (rise)
        {
            if (riseTimer < 0)
            {
                collision.Move(riseSpeed * Time.deltaTime, Vector2.zero);
            }
            riseTimer -= Time.deltaTime;
        }

        
        if(collision.collisions.below)
        {            
            rise = true;
            drop = false;
            
        }

        if(collision.collisions.above)
        {
            drop = false;
            rise = false;
            reset = true;
        }
	}



    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && reset)
        {
            drop = true;
            reset = false;
            riseTimer = waitTime;
        }
    }
}