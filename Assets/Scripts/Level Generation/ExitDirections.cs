//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExitDirections
{
    //Get the opposite direction for room transitions
	 public static Direction GetOpposite(this Direction direction)
    {
        switch(direction)
        {
            case Direction.North:
                return Direction.South;

            case Direction.East:
                return Direction.West;

            case Direction.South:
                return Direction.North;

            case Direction.West:
                return Direction.East;

            //Error
            default:
                throw new System.Exception("Direction: " + direction + " not found.");
        }
    }

    //Get the grid position of a room
    public static Vector2Int GetCoordinates(this Direction direction, Vector2Int position)
    {
        Vector2Int newPosition = position;

        switch (direction)
        {
            case Direction.North:
                newPosition.y++;
                return newPosition;

            case Direction.East:
                newPosition.x++;
                return newPosition;

            case Direction.South:
                newPosition.y--;
                return newPosition;

            case Direction.West:
                newPosition.x--;
                return newPosition;
            
            //Error
            default:
                throw new System.Exception("Direction: " + direction + " not found.");
        }
    }
}

//Directions
public enum Direction
{
    North,
    East,
    South,
    West,
    None
}
