//Created by Robert Bryant
//
//Allows entities to climb on certain game objects
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableController : MonoBehaviour
{
    CollisionController[] collision;        //References to each CollisionController

    private void Start()
    {
        //Find the player and enemies
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int x = enemies.Length + 2;

        //Create a new array for collision
        collision = new CollisionController[x];
        collision[0] = player.GetComponent<CollisionController>();
    }


    private void OnTriggerStay(Collider other)
    {
        //The player is touching the climbable object
        if(other.tag =="Player")
        {
            collision[0].collisions.canClimb = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //The player is no longer touching the climbable object
        if (other.tag == "Player")
        {
            collision[0].collisions.canClimb = false;
            collision[0].collisions.climbingObject = false;
        }
    }
}