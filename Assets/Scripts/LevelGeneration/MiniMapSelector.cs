using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapSelector : MonoBehaviour
{
    public GameObject miniMapObject;                //Parent for the mini map rooms
    public GameObject[] exitPrefab;                 //Visual representation of the exits
    public Direction[] exits = new Direction[4];    //Direction of each exit
    public RoomType roomType;                       //Room type

    [Header("Mini Map Colors")]
    public Color entrance;                          //Color to show a room has been entered
    public Color room;                              //Default room color
    public Color treasure;
    public Color boss;
    public Color mystery;
    public Gradient currentRoom;                    //Current room color

    [Header("Gizmos")]
    public Color gizmoColor;


    private MiniMapCameraController miniMapFocus;    //Reference to the mini map camera
    private float color;                             //Number for the gradien color value

    private void Start()
    {
        //Get the mini map camera
        miniMapFocus = GameObject.FindGameObjectWithTag("MiniMapCamera").GetComponent<MiniMapCameraController>();
        SelectRoomColor();
        SelectRoomExits();
        //HideRoom();
    }

    //Select the color of the room based on the type of room
    void SelectRoomColor()
    {
        switch(roomType)
        {
            case RoomType.Entrance:
                miniMapObject.GetComponent<Renderer>().material.color = entrance;

                foreach (var item in exitPrefab)
                {
                    item.GetComponent<Renderer>().material.color = entrance;
                }
                break;

            case RoomType.Treasure:
                miniMapObject.GetComponent<Renderer>().material.color = treasure;
                foreach (var item in exitPrefab)
                {
                    item.GetComponent<Renderer>().material.color = treasure;
                }
                break;

            case RoomType.Boss:
                miniMapObject.GetComponent<Renderer>().material.color = boss;
                foreach (var item in exitPrefab)
                {
                    item.GetComponent<Renderer>().material.color = boss;
                }
                break;

            case RoomType.Mystery:
                miniMapObject.GetComponent<Renderer>().material.color = mystery;
                foreach (var item in exitPrefab)
                {
                    item.GetComponent<Renderer>().material.color = mystery;
                }
                break;

            case RoomType.None:
                miniMapObject.GetComponent<Renderer>().material.color = room;
                foreach (var item in exitPrefab)
                {
                    item.GetComponent<Renderer>().material.color = room;
                }
                break;

            default:
                throw new System.Exception("Room type not set on " + transform.name);
        }
    }

    //Set the rooms exits
    void SelectRoomExits()
    {
        //Loop through each exit
        for (int i = 0; i < exits.Length; i++)
        {
            //If there is an exit direction set the prefab to be active
            if (exits[i] != Direction.None)
            {
                exitPrefab[i].SetActive(true);
            }
        }
    }

    //Hides the room
    void HideRoom()
    {
        //Loop through each exit and hide them
        for(int i = 0; i < exits.Length; i++)
        {
            if(exits[i] != Direction.None)
            {
                exitPrefab[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
        miniMapObject.GetComponent<MeshRenderer>().enabled = false;
    }
    
    //When something enters the room
    private void OnTriggerEnter(Collider other)
    {
        //If the player enters
        if (other.tag == "Player")
        {
            //Show the room and each exit
            for (int i = 0; i < exits.Length; i++)
            {
                exitPrefab[i].GetComponent<MeshRenderer>().enabled = true;
            }
            miniMapObject.GetComponent<MeshRenderer>().enabled = true;

            //If the room is not currently the focus set it as the focus for the mini map
            if(miniMapFocus != transform)
            {
                miniMapFocus.cameraTarget = transform;
            }
        }
    }

    //While something is in the room
    private void OnTriggerStay(Collider other)
    {
        //If the player is in the room
        if (other.tag == "Player")
        {
            //If the mini map room is hidden, show the room
            if (miniMapObject.GetComponent<MeshRenderer>().enabled == false)
            {
                for (int i = 0; i < exits.Length; i++)
                {
                    exitPrefab[i].GetComponent<MeshRenderer>().enabled = true;
                }
                miniMapObject.GetComponent<MeshRenderer>().enabled = true;
            }

            //Set the color of the room
            for (int i = 0; i < exits.Length; i++)
            {
                exitPrefab[i].GetComponent<MeshRenderer>().material.color = currentRoom.Evaluate(color);
            }
            miniMapObject.GetComponent<MeshRenderer>().material.color = currentRoom.Evaluate(color);
            
            //Change the color while in the room
            color += 0.01f;
            if(color > 1)
            {
                color = 0;
            }
        }

    }

    //When something leaves the room
    private void OnTriggerExit(Collider other)
    {
        //When the player leaves reset the room to the default color
        if (other.tag == "Player")
        {
            SelectRoomColor();
        }
    }
    
    //Mini Map Room Gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}