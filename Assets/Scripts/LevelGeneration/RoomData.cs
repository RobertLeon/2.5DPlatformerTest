using UnityEngine;

public class RoomData
{
    public Vector2 gridPos;                             //Room's position on the grid
    public Vector2 roomSize;                            //Size of the room

    public RoomType roomType;                           //Room's type

    public Direction[] exits =                          //Room's exits
        new Direction[4]; 
    
    //Constructor for the room's data
    public RoomData(Vector2 gridPos, RoomType roomType, Vector2 roomSize)
    {
        this.gridPos = gridPos;
        this.roomType = roomType;
        this.roomSize = roomSize;
    }

    //Get the amount of exits for a room
    public int ExitCount()
    {
        int count = 0;

        //Loop through each exit
        for(int i = 0; i < exits.Length; i++)
        {
            //If the exit has a direction
            if(exits[i] != Direction.None)
            {
                count++;
            }
        }
        
        return count;
    }    
}

//Each type of room
public enum RoomType
{
    None,
    Entrance,
    Boss,
    Treasure,
    Mystery
}

//Each possible direction a room can have as an exit
public enum Direction
{
    None,
    North,
    East,
    South,
    West
}