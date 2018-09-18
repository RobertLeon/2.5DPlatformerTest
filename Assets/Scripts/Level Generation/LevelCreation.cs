//Created by Robert Bryant
//
//Creates levels procedurally
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelCreation : MonoBehaviour
{
    public Vector2Int levelSize;                //Maximum level size    
    public int minRoomsToExit;                  //Minimum rooms to the exit
    public int maxRoomsToExit;                  //Maximum rooms to the exit
    public Vector2 roomSize;                    //Size of the rooms
    public RoomController[] startRooms;         //Startrooms to choose from
    public RoomController[] rooms;              //Basic rooms to choose from
    public RoomController[] exitRooms;          //Exit rooms to choose from

    private int minRoomCount;                    //Minimum amount of rooms for the current level
    private Vector2Int gridSize;                //Size of the grid
    
    //Collection of rooms and positions
    private Dictionary<RoomController, Vector2Int> roomCoordinates;     
    

    // Use this for initialization
    void Start()
    {
        //Shrink the level size by 1
        gridSize = levelSize - Vector2Int.one;

        //Minimum amount of rooms for a level to create
        minRoomCount = (levelSize.x * levelSize.y) / 2;

        //Create the level
        CreateRooms();
    }

    //Creates the rooms in the level
    public void CreateRooms()
    {
        //Get a new dictionaries
        roomCoordinates = new Dictionary<RoomController, Vector2Int>();
        Dictionary<RoomController, List<Direction>> branchingRooms = new Dictionary<RoomController, List<Direction>>();

        RoomController currentRoom = null;                      //Current room
        List<Direction> currentExits = new List<Direction>();   //Current exits        

        //Create a path to the exit
        CreatePathToExit(GetRandomGridPosition());

        //Loop for each room in the level
        for (int x = 0; x < minRoomCount; x++)
        {
            //Loop through rach room that has been placed
            foreach (KeyValuePair<RoomController, Vector2Int> placedRooms in roomCoordinates.ToArray())
            {
                //Set the current room and make a new list of exits
                currentRoom = placedRooms.Key;
                currentExits = new List<Direction>();

                //If the room is not in the dictionary
                if (!branchingRooms.ContainsKey(placedRooms.Key))
                {
                    //Check if the room has more than one exit
                    if (placedRooms.Key.roomData.exits.Length > 1)
                    {
                        //Loop through each exit in the room
                        for (int i = 0; i < placedRooms.Key.roomData.exits.Length; i++)
                        {
                            //Check for a room to the north and if the room has an exit to the north
                            if (!roomCoordinates.ContainsValue(
                                currentRoom.roomData.gridPosition + Vector2Int.up) &&
                                placedRooms.Key.roomData.exits[i] == Direction.North)
                            {
                                currentExits.Add(placedRooms.Key.roomData.exits[i]);
                            }
                            //Check for a room to the east and if the room has an exit to the east
                            else if (!roomCoordinates.ContainsValue(
                                currentRoom.roomData.gridPosition + Vector2Int.right) &&
                                placedRooms.Key.roomData.exits[i] == (Direction.East))
                            {
                                currentExits.Add(placedRooms.Key.roomData.exits[i]);
                            }
                            //Check for a room to the south and if the room has an exit to the south
                            else if (!roomCoordinates.ContainsValue(
                               currentRoom.roomData.gridPosition + Vector2Int.down) &&
                               placedRooms.Key.roomData.exits[i] == (Direction.South))
                            {
                                currentExits.Add(placedRooms.Key.roomData.exits[i]);
                            }
                            //Check for a room to the west and if the room has an exit to the west
                            else if (!roomCoordinates.ContainsValue(
                                currentRoom.roomData.gridPosition + Vector2Int.left) &&
                                placedRooms.Key.roomData.exits[i] == (Direction.West))
                            {
                                currentExits.Add(placedRooms.Key.roomData.exits[i]);
                            }
                        }

                        //Add the room to the dictionary
                        branchingRooms.Add(placedRooms.Key, currentExits);
                    }
                }
            }
           
            //Reset the current room
            currentRoom = null;
            
            //Loop through each room in the path
            foreach (KeyValuePair<RoomController, List<Direction>> path in branchingRooms)
            {
                //Loop through each room transition
                foreach (Direction transition in path.Value)
                {
                    //Get the position of the room
                    Vector2Int gridPos = transition.GetCoordinates(path.Key.roomData.gridPosition);

                    //If there is no room at that position create a room
                    if (!roomCoordinates.ContainsValue(gridPos))
                    {
                        //Get the room
                        currentRoom = GetRandomRoom(rooms, gridPos, transition.GetOpposite(), Direction.None);
                        
                        //Create the room
                        currentRoom = Instantiate(currentRoom, gridPos * roomSize,
                            Quaternion.identity, transform);
                       
                        //Rename the room
                        currentRoom.name = currentRoom.roomData.roomType + " " +
                            currentRoom.roomData.GetExitDirections(currentRoom.roomData.exits)
                            + " " + gridPos.ToString();
                       
                        //Set the room's grid position
                        currentRoom.roomData.gridPosition = gridPos;

                        //Add the room to the dictionary
                        roomCoordinates.Add(currentRoom, gridPos);
                    }
                }
            }
        }

        //Check if the minimum amount of rooms has spawned
        if (roomCoordinates.Keys.Count < (minRoomCount / 2))
        {
            Debug.LogWarning("Not enough rooms spawned");
            List<RoomController> roomsToRemove = new List<RoomController>(GetComponentsInChildren<RoomController>());

            //Destroy each room that was created
            foreach (RoomController room in roomsToRemove.ToArray())
            {
                Destroy(room.gameObject);
            }

            //Create the level again
            CreateRooms();
        }
    }
       
    //Get a random position for the rooms
    private Vector2Int GetRandomGridPosition()
    {
        Vector2Int position = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));

        //Check if a room contains the position
        if (!roomCoordinates.ContainsValue(position))
        {
            return position;
        }
        //If it does try to get another random position
        else
        {
            return GetRandomGridPosition();
        }
    }

    //Create a random path to the exit
    private Dictionary<Vector2Int, Direction> PathToExit(Vector2Int pos)
    {
        //Starting position of the path
        Vector2Int pathPos = pos;

        //Get a random number for the amount of spaces inbetween the start and end rooms
        int spaces = Random.Range(minRoomsToExit + 1, maxRoomsToExit);
        
        //Direction moved in the path
        Direction moveDir = Direction.None;
        
        //Path of coordinates and directions moved
        Dictionary<Vector2Int, Direction> path = new Dictionary<Vector2Int, Direction>();

        //Loop through each space in the path
        for (int i = 0; i <= spaces; i++)
        {
            //Make a new list of possible moves towards the exit
            List<Direction> moves = new List<Direction>();

            //Check if it is possible to move north and the previous movement was not to the south
            if (pathPos.y + 1 < gridSize.y && moveDir != Direction.South)
            {
                moves.Add(Direction.North);
            }

            //Check if it is possible to move east and the previous movement was not to the west
            if (pathPos.x + 1 < gridSize.x && moveDir != Direction.West)
            {
                moves.Add(Direction.East);
            }

            //Check if it is possible to move south and the previous movement was not to the north
            if (pathPos.y - 1 > 0 && moveDir != Direction.North)
            {
                moves.Add(Direction.South);
            }

            //Check if it is possible to move west and the previous movement was not to the east
            if (pathPos.x - 1 > 0 && moveDir != Direction.East)
            {
                moves.Add(Direction.West);
            }

            //Check if it is possible to keep moving
            if (moves.ToArray().Length != 0)
            {
                //Pick a random direction from the list
                moveDir = moves[Random.Range(0, moves.ToArray().Length)];
            }
            else
            {
                //Look for a new path
                return PathToExit(pos);
            }

            //Update the position based on the direction chosen
            switch (moveDir)
            {
                case Direction.North:
                    pathPos.y++;
                    break;

                case Direction.East:
                    pathPos.x++;                    
                    break;

                case Direction.South:
                    pathPos.y--;                    
                    break;

                case Direction.West:
                    pathPos.x--;                    
                    break;
            }

            //Check if the path has double backed on itself or gone back to the start
            if(path.Keys.Contains(pathPos) || pathPos == pos)
            {
                return PathToExit(pos);
            }
            else
            {
                //Add the current position and direction to the dictionary
                path.Add(pathPos, moveDir);
            }
        }

        return path;
    }


    //Create rooms in a path from the entrance position to the exit position
    private void CreatePathToExit(Vector2Int entrancePos)
    {
        Dictionary<Vector2Int, Direction> pathToExit = new Dictionary<Vector2Int, Direction>();
        pathToExit = PathToExit(entrancePos);

        if (pathToExit.FirstOrDefault().Key != null)
        {
            //Set the transition for the entrance
            Direction transition = pathToExit.FirstOrDefault().Value.GetOpposite();
            RoomController newRoom = null;

            //Create the entrance room
            CreateEntrance(pathToExit.First().Key, pathToExit.First().Value);

            CreateExit(pathToExit.Last().Key, pathToExit.Last().Value.GetOpposite());

            //Loop through each position in the path and create a room
            foreach (KeyValuePair<Vector2Int, Direction> path in pathToExit)
            {
                //Check if a room exists in that position
                if (!roomCoordinates.ContainsValue(path.Key))
                {
                    //Get a random room
                    newRoom = GetRandomRoom(rooms, path.Key, transition, path.Value);

                    //Create the random room
                    newRoom = Instantiate(newRoom, path.Key * roomSize, Quaternion.identity, transform);

                    //Change the room's name
                    newRoom.name = newRoom.roomData.roomType + " " +
                        newRoom.roomData.GetExitDirections(newRoom.roomData.exits) + " " + path.Key.ToString();

                    //Set the room's grid position
                    newRoom.roomData.gridPosition = path.Key;

                    //Add the room to the dictionary
                    roomCoordinates.Add(newRoom, path.Key);

                    //Set the transition for the next room
                    transition = path.Value.GetOpposite();
                }
            }
        }
    }
    
    //Checks the position of the room and alligns it to the grid
    private List<RoomController> AllignToGrid(RoomController[] rooms, Vector2Int gridPos)
    {
        //Create a new list of rooms
        List<RoomController> newRooms = new List<RoomController>(rooms);

        //Top Right Corner
        if (gridPos.x + 1 > gridSize.x && gridPos.y + 1 > gridSize.y)
        {
            //Loop through each room
            foreach (RoomController room in newRooms.ToArray())
            {
                //Remove rooms that contain exits to the north or east
                if (room.roomData.exits.Contains(Direction.North) ||
                    room.roomData.exits.Contains(Direction.East))
                {
                    newRooms.Remove(room);
                }
            }
        }
        //Bottom Right Corner
        else if (gridPos.x + 1 > gridSize.x && gridPos.y - 1 < 0)
        {
            //Loop through each room
            foreach (RoomController room in newRooms.ToArray())
            {
                //Remove rooms that contain exits to the south or east
                if (room.roomData.exits.Contains(Direction.South) ||
                    room.roomData.exits.Contains(Direction.East))
                {
                    newRooms.Remove(room);
                }
            }
        }
        //Top Left Corner
        else if (gridPos.x - 1 < 0 && gridPos.y + 1 > gridSize.y)
        {
            //Loop through each room
            foreach (RoomController room in newRooms.ToArray())
            {
                //Remove rooms that contain exits to the north or west
                if (room.roomData.exits.Contains(Direction.North) ||
                     room.roomData.exits.Contains(Direction.West))
                {
                    newRooms.Remove(room);
                }
            }
        }
        //Bottom Left Corner
        else if (gridPos.x - 1 < 0 && gridPos.y - 1 < 0)
        {
            //Loop through each room
            foreach (RoomController room in newRooms.ToArray())
            {
                //Remove rooms that contain exits to the south or west
                if (room.roomData.exits.Contains(Direction.South) ||
                     room.roomData.exits.Contains(Direction.West))
                {
                    newRooms.Remove(room);
                }
            }
        }
        //Top Row
        else if (gridPos.y + 1 > gridSize.y)
        {
            //Loop through each room
            foreach (RoomController room in newRooms.ToArray())
            {
                //Remove rooms that contain exits to the north
                if (room.roomData.exits.Contains(Direction.North))                     
                {
                    newRooms.Remove(room);
                }
            }
        }
        //Bottom Row
        else if (gridPos.y - 1 < 0)
        {
            //Loop through each room
            foreach (RoomController room in newRooms.ToArray())
            {
                //Remove rooms that contain exits to the south
                if (room.roomData.exits.Contains(Direction.South))
                {
                    newRooms.Remove(room);
                }
            }
        }
        //Right most collumn
        else if (gridPos.x + 1 > gridSize.x)
        {
            //Loop through each room
            foreach (RoomController room in newRooms.ToArray())
            {
                //Remove rooms that contain exits to the east
                if (room.roomData.exits.Contains(Direction.East))
                {
                    newRooms.Remove(room);
                }
            }
        }
        //Left most collumn
        else if (gridPos.x - 1 < 0)
        {
            //Loop through each room
            foreach (RoomController room in newRooms.ToArray())
            {
                //Remove rooms that contain exits to the west
                if (room.roomData.exits.Contains(Direction.West))
                {
                    newRooms.Remove(room);
                }
            }
        }
        
        //Return the rooms that are left
        return newRooms;
    }

    //Returns a random room with the specified tansition and/or exit
    private RoomController GetRandomRoom(RoomController[] rooms, Vector2Int gridPos, Direction transition, Direction exit)
    {
        //Create a new list of rooms
        List<RoomController> newRooms = new List<RoomController>();

        //If there is no transition into the room
        if (transition == Direction.None)
        {
            //Loop through each room
            foreach (RoomController room in rooms)
            {
                //Loop through each exit in the room
                foreach (Direction dir in room.roomData.exits)
                {
                    //If the direction is the same as the exit add it to the list
                    if (dir == exit)
                    {
                        newRooms.Add(room);
                    }
                }
            }
        }
        //If there id no exit specified
        else if (exit == Direction.None)
        {
            //Loop through each room
            foreach (RoomController room in rooms)
            {
                //Loop through each exit in the room
                foreach (Direction dir in room.roomData.exits)
                {
                    //if the direction is the same as the transition add it to the list
                    if (dir == transition)
                    {
                        newRooms.Add(room);
                    }
                }
            }
        }
        //If both a transition and exit is specified
        else
        {
            //Loop through each room
            foreach (RoomController room in rooms)
            {
                List<Direction> transitions = new List<Direction>(room.roomData.exits);

                //If the room has both the transition and exit add it to the list
                if (transitions.Contains(transition) && transitions.Contains(exit))
                {
                    newRooms.Add(room);
                }
            }
        }

        //Allign the rooms to the level grid
        List<RoomController> allignedRooms = new List<RoomController>();
        allignedRooms = AllignToGrid(newRooms.ToArray(), gridPos);

        //Return a random room
        if (allignedRooms.ToArray().Length == 0)
        {
            return null;
        }
        else
        {
            return allignedRooms[Random.Range(0, allignedRooms.ToArray().Length)];
        }
    }

    //Creates the entrance room for the level
    private RoomController CreateEntrance(Vector2Int gridPos, Direction exit)
    {
        Vector2 roomPos;                    //Position of the room
        RoomController newRoom = null;      //The room object

        //Position of the entrance room
        roomPos = gridPos * roomSize;

        //Create the entrance room
        newRoom = Instantiate(GetRandomRoom(startRooms, gridPos, Direction.None, exit), roomPos,
            Quaternion.identity, transform);

        //Change the name of the transform and add it to the dictionary
        newRoom.name = newRoom.roomData.roomType + " " +
            newRoom.roomData.GetExitDirections(newRoom.roomData.exits) + " " + gridPos.ToString();
        newRoom.roomData.gridPosition = gridPos;
        roomCoordinates.Add(newRoom, gridPos);

        //Return the room
        return newRoom;
    }

    //Create an exit for the level
    private RoomController CreateExit(Vector2Int gridPos, Direction transition)
    {
        Vector2 roomPos;                    //Position of the exit
        RoomController newRoom = null;      //Room object

        //Position of the exit room
        roomPos = gridPos * roomSize;

        //Create the exit room
        newRoom = Instantiate(GetRandomRoom(exitRooms, gridPos, transition,Direction.None), roomPos,
            Quaternion.identity, transform);

        //Change the name of the transform and add it to the dictionary
        newRoom.name = newRoom.roomData.roomType + " " +
            newRoom.roomData.GetExitDirections(newRoom.roomData.exits) + " " + gridPos.ToString();
        newRoom.roomData.gridPosition = gridPos;
        roomCoordinates.Add(newRoom, gridPos);

        //Return the room
        return newRoom;
    }
}