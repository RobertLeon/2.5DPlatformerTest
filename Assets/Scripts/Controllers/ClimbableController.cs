//Created by Robert Bryant
//
//Allows entities to climb on certain game objects
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(Rigidbody2D))]
public class ClimbableController : MonoBehaviour
{
    CollisionController[] collisionController;        //References to each CollisionController

    private void Start()
    {
        //Find the player and enemies
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int x = enemies.Length + 2;

        //Create a new array for collision
        collisionController = new CollisionController[x];
        collisionController[0] = player.GetComponent<CollisionController>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //The player is touching the climbable object
        if (collision.tag == "Player")
        {
            //If the player is not sliding
            if(!collisionController[0].collisions.sliding)
            {
                collisionController[0].collisions.canClimb = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //The player is no longer touching the climbable object
        if (collision.tag == "Player")
        {
            collisionController[0].collisions.canClimb = false;
            collisionController[0].collisions.climbingObject = false;
        }
    }    
}