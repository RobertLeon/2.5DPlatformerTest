using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableController : MonoBehaviour
{
    public Transform topObstacle;

    CollisionController[] collision;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int x = enemies.Length + 2;

        collision = new CollisionController[x];
        collision[0] = player.GetComponent<CollisionController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag =="Player")
        {
            collision[0].collisions.canClimb = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            collision[0].collisions.canClimb = false;
            collision[0].collisions.climbingObject = false;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.GetComponent<BoxCollider>().size);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(topObstacle.position, topObstacle.localScale);
    }
}