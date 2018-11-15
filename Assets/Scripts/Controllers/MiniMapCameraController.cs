//Created by Robert Bryant
//
//Moves the minimap camera based on the room the player is currently in
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraController : MonoBehaviour
{
    public Transform cameraTarget;              //Target for the camera
    
    public Vector3 cameraOffset;                //Camera offset
    public float zoomAmount = 50f;
    public float minSize = 50f;
    public float maxSize = 500f;

    //private MiniMapSelector[] miniMapRooms;     //Array to hold the rooms in the mini map
    private InputManager inputManager;
    private Camera miniMapCam;
    private Transform miniMap;                   //Mini map parent

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        miniMapCam = GetComponent<Camera>();
        miniMap = GameObject.Find("MiniMap").transform;

        //Get each mini map room
        //miniMapRooms = miniMap.GetComponentsInChildren<MiniMapSelector>();

        //Loop through each room
        /*foreach (var room in miniMapRooms)
        {
            //Set the camera target to the specified room
            if(room.roomType == RoomType.Entrance)
            {
                cameraTarget = room.transform;
            }
        } */  

    }


    //Update is called once per frame
    void LateUpdate()
    {
        //If the camera target exists move to that position
        if (cameraTarget != null)
        {
            transform.position = cameraTarget.transform.position + cameraOffset;
        }

        
        //Zoom the camera out
        if (inputManager.GetKeyDown("Zoom Out") || inputManager.GetButtonDown("Zoom Out"))
        {
            ZoomOut();
        }

        //Zoom the camera in
        if (inputManager.GetKeyDown("Zoom In") || inputManager.GetKeyDown("Zoom In"))
        {
            ZoomIn();
        }
        
    }

    //Zoom the camera in
    public void ZoomIn()
    {
        //Check if the camera can be zoomed in
        if (miniMapCam.orthographicSize > minSize)
        {
            miniMapCam.orthographicSize -= zoomAmount;
        }
    }

    //Zoom the camera out
    public void ZoomOut()
    {
        //Check if the camera can be zoomed out
        if (miniMapCam.orthographicSize < maxSize)
        {
            miniMapCam.orthographicSize += zoomAmount;
        }
    }
}