//Created by Robert Bryant
//
//Used by the room prefabs to know which room 
//should be spawned based on assigned parameters
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    public Vector2 gridPosition;        //Room's position on the grid
    public Vector2 roomSize;            //Size of the room
    public RoomType roomType;           //The room's type
    public Direction[] exits;           //Exits the room has
    
    [Header("Gizmos")]
    public Color gizmoColor;            //Gizmo color

    //Set up the room's informaiton
    public void SetUp(Vector2 gridPosition, RoomType roomType)
    {
        this.gridPosition = gridPosition;
        this.roomType = roomType;
    }

    //Get the amount of exits in a room
    public int ExitCount()
    {
        int count = 0;
        for(int i = 0; i < exits.Length; i++)
        {
            if(exits[i] != Direction.None)
            {
                count++;
            }
        }
        return count;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}