//Created by Robert Bryant
//
//Handles the activation of the camera boundary and enemy logic
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public RoomData roomData;                           //Data of this room   

    private BoxCollider boxCollider;                    //Reference to the Box Collider
    private CameraController cameraController;          //Reference to the Camera Controller

    //Use this for initialization
    void Start()
	{
        //Get the box collider and camera controller scripts
        boxCollider = transform.GetComponent<BoxCollider>();
        cameraController = Camera.main.GetComponent<CameraController>();
	}

    //When something enters the room
    private void OnTriggerEnter(Collider other)
    {
        //The player enters the room
        if(other.tag == "Player")
        {
            //Set the camera boundaries for the current room
            cameraController.SetCameraBoundary(boxCollider);          

            //Activate enemys
        }
    }

    //When something exits the room
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            
            //Deactavate enemys
        }
    }

    //Update is called once per frame
    void Update()
	{
		
	}


    //Draws gizmnos in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = roomData.GetRoomColor();
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
