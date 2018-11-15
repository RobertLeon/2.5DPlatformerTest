//Created by Robert Bryant
//
//Handles the activation of the camera boundary and enemy logic
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public RoomData roomData;                           //Data of this room   

    private BoxCollider2D boxCollider;                  //Reference to the Box Collider
    private CameraController cameraController;          //Reference to the Camera Controller

    //Use this for initialization
    void Start()
	{
        //Get the box collider and camera controller scripts
        boxCollider = transform.GetComponent<BoxCollider2D>();
        cameraController = Camera.main.GetComponent<CameraController>();
	}
    //When something enters the room
    private void OnTriggerEnter2D(Collider2D collision)
    {   //The player enters the room
        if(collision.tag == "Player")
        {
            //Set the camera boundaries for the current room
            cameraController.SetCameraBoundary(boxCollider); 
            
            //Activate enemys
        }
    }

    //When something exits the room
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Deactavate enemys
        }
    }

    //Draws gizmnos in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = roomData.GetRoomColor();
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
