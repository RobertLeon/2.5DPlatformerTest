//Created by Robert Bryant
//
//Holds a rooms data for procedural generation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public RoomType roomType;                       //Designates the type of room this is
    public Direction[] exits = new Direction[4];    //How many exits and which direction they are in
    public Vector2Int gridPosition;                 //Position of the room on the grid

    private  Color roomColor;                       //Color of the room based on room type

    //Returns the directions a room has
    public string GetExitDirections(Direction[] exits)
    {
        string compass = "";
        
        //Room has all exits
        if (exits.Length == 4)
        {
            return "NESW";
        }
        else
        {
            //Loop through each exit and add it to the string.
            for (int i = 0; i < exits.Length; i++)
            {
                if (exits[i] == Direction.North)
                {
                    compass += "N";
                }
                else if (exits[i] == Direction.East)
                {
                    compass += "E";
                }
                else if (exits[i] == Direction.South)
                {
                    compass += "S";
                }
                else if (exits[i] == Direction.West)
                {
                    compass += "W";
                }
            }
            return compass;
        }
    }
    
    //Get the color of the room based on room type
    public Color GetRoomColor()
    {
        switch(roomType)
        {
            //Entrance is green
            case RoomType.Entrance:
                return roomColor = new Color(0f, 1f, 0f, .25f);
            
            //Exit is red
            case RoomType.Exit:
                return roomColor = new Color(1f, 0f, 0f, .25f);
            
            //Basic rooms are white
            case RoomType.None:
                return roomColor = new Color(1f, 1f, 1f, .25f);
            
            //Treasure rooms are yellow
            case RoomType.Treasure:
                return roomColor = new Color(0f, .5f, .5f, 0.25f);
            
            //Error
            default:
                throw new System.Exception("Room type: " + roomType + " not found");
        }
    }

}
//Room Types
public enum RoomType
{
    None,
    Entrance,
    Exit,
    Treasure
}

