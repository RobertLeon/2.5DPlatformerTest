//Created by Robert Bryant
//
//Allows entities to climb on certain game objects
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ClimbableController : MonoBehaviour
{
    public delegate void Climable(bool canClimb);
    public static event Climable ClimbState;


    private void OnTriggerStay(Collider other)
    {
        //The player is touching the climbable object
        if (other.tag == "Player")
        {
            ClimbState.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //The player is no longer touching the climbable object
        if (other.tag == "Player")
        {
            ClimbState.Invoke(false);
        }
    }    
}