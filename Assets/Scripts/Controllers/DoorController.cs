//Created by Robert Bryant
//
//Opens and closes doors
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public Vector3 rotation;                //Rotation for the door when open
    public float openTime = 5f;             //Time the door stays open

    private bool isOpen = false;            //Check if the door is open
    private float doorTimer;                //Timer for the door

	// Use this for initialization
	void Start ()
    {
        //Set door timer to the open time
        doorTimer = openTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If the door is open
		if(isOpen)
        {
            //Count down on timer
            doorTimer -= Time.deltaTime;
            
            //Reset the door to the original rotation
            if(doorTimer < 0f)
            { 
                transform.localRotation = Quaternion.identity;
                doorTimer = openTime;
                isOpen = false;
            }
        }
	}

    //Opens the door when triggered
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            transform.localRotation = Quaternion.Euler(rotation);
            isOpen = true;
        }
    }

}
