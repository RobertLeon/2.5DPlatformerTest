//Created by Robert Bryant
//
//Moves the minimap camera based on the room the player is currently in
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraController : MonoBehaviour
{
    public Transform cameraTarget;              //Target for the camera
    public Transform miniMap;                   //Mini map parent
    public Vector3 cameraOffset;                //Camera offset

    //private MiniMapSelector[] miniMapRooms;     //Array to hold the rooms in the mini map

    private void Start()
    {
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
    }
}