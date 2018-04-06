//Created by Robert Bryant
//Based off of a tutorial by: Six Dot Studios
//https://drive.google.com/file/d/1Wd5vvpF3IzREKAs5iofKisB1gV2dWAZ8/view
//Generates a procedural level using preset rooms
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomAssigner))]
public class LevelGeneration : MonoBehaviour
{
    public Vector2 worldSize;                               //Size of the world
    public Vector2 roomSize;                                //Size of the rooms
    public RoomData[,] rooms;                               //Data for each room position
    public int numberOfRooms = 20;                          //Number of rooms to make
    [Range(0.01f, 1.0f)]
    public float startComparison = 0.2f;                    //Comparison numbers for branching
    [Range(0.01f, 1.0f)]                                    //paths
    public float endComparison = 0.01f;
    public Transform miniMapParent;                         //Mini map parent gameobject
    public Transform miniMapRoom;                           //Minimap room prefab
    [HideInInspector]
    public bool complete = false;                           //Check if the level has completed genration

    private List<Vector2> takenPositions =                  //Positions taken in the map
        new List<Vector2>();
    private int gridSizeX;                                  //Size of the grid on the X-axis    
    private int gridSizeY;                                  //Size of the grid on the Y-axis
    private bool bossRoom = false;
   

    //Use this for initialization
    void Start()
    {
        //Limit the number of rooms to the world size
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }

        //Set the grid size
        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);

        //Generate the room data and create both the mini map and level
        CreateRooms();
        SetRoomExits();
        DrawMap();
        transform.GetComponent<RoomAssigner>().Assign(rooms);
    }


    private void Update()
    {
        //If the level is generated withouth a boss room
        //destroy all the rooms and start over.
        /*if(complete && !bossRoom)
        {
            Debug.LogError("Map generated without boss room");
            complete = false;
            DestroyRooms();
            RestartGeneration();            
        }*/
    }

    //Restarts the level genration
    void RestartGeneration()
    {
        CreateRooms();
        SetRoomExits();
        DrawMap();
        transform.GetComponent<RoomAssigner>().Assign(rooms);
    }

    //Destroys the created rooms
    void DestroyRooms()
    {
        RoomInstance[] rooms = GetComponentsInChildren<RoomInstance>();

        foreach(var room in rooms)
        {
            Destroy(room.gameObject);
        }

        MiniMapSelector[] minimap = GetComponentsInChildren<MiniMapSelector>();

        foreach(var mini in minimap)
        {
            Destroy(mini.gameObject);
        }

        takenPositions.Clear();
        bossRoom = false;
    }

    //Draw the mini map
    void DrawMap()
    {
        int roomNum = 1;
        int mRooms = 0;

        //Loop through each room
        foreach(RoomData room in rooms)
        {
            bool mystery = (mRooms >= 4);
           
            //If the room does not exists go to the next room 
            if(room == null)
            {
                continue;
            }

            //Set the room position
            Vector2 roomPos = room.gridPos;
            roomPos.x *= room.roomSize.x;
            roomPos.y *= room.roomSize.y;

            //Create the room
            MiniMapSelector mapper = Instantiate(miniMapRoom,
                roomPos, Quaternion.identity, miniMapParent).GetComponent<MiniMapSelector>();

            if (room.roomType != RoomType.Entrance)
            {
                if (room.ExitCount() == 1)
                {
                    room.roomType = RoomType.Treasure;
                }/*
                else if (Random.value > 0.8f && !mystery)
                {
                    room.roomType = RoomType.Mystery;
                    mRooms++;
                }
                else if(room.ExitCount() == 2)
                {
                    room.roomType = RoomType.Corridor;
                }
                else if (room.ExitCount() >= 2 && Random.value > 0.5f && !bossRoom)
                {
                    bossRoom = true;
                    room.roomType = RoomType.Boss;
                }*/
            }

            //Set the mini map room's type
            mapper.roomType = room.roomType;

            //Set the mini map room's exits to match the current room
            for (int i = 0; i < room.exits.Length; i++)
            {
                mapper.exits[i] = room.exits[i];
            }

            //Change the mini map's name from the prefab
            mapper.name = "Mini Map " + roomNum + " " + room.roomType;

            //Set the scale of the minimap to the room size
            mapper.transform.localScale = room.roomSize;

            roomNum++;
        }

        complete = true;
    }


    //Set the exits of a room
    void SetRoomExits()
    {
        //Loop through each room in the grid
        for (int x = 0; x < (gridSizeX * 2); x++)
        {
            for (int y = 0; y < (gridSizeY * 2); y++)
            {
                //If the room doesn't exist move to the next room
                if (rooms[x, y] == null)
                {
                    continue;
                }
                
                //Check for a room to the North
                if (y + 1 >= (gridSizeY * 2))
                {
                    //If at the northern edge of the map
                    rooms[x, y].exits[0] = Direction.None;
                }
                else
                {
                    //Check for a room to the North of the current room
                    rooms[x, y].exits[0] = (rooms[x, y + 1] != null) ? Direction.North : Direction.None;
                }

                //Check for a room to the East
                if (x + 1 >= (gridSizeX * 2))
                {
                    //If at the eastern edge of the map
                    rooms[x, y].exits[1] = Direction.None;
                }
                else
                {
                    //Check for a room to the east of the current room
                    rooms[x, y].exits[1] = (rooms[x + 1, y] != null) ? Direction.East : Direction.None;
                }

                //Check for a room to the South
                if (y - 1 < 0)
                {
                    //If at the southern edge of the map
                    rooms[x, y].exits[2] = Direction.None;
                }
                else
                {
                    //Check for a room to the south of the current room
                    rooms[x, y].exits[2] = (rooms[x, y - 1] != null) ? Direction.South : Direction.None;
                }

                //Check for a room to the West
                if (x - 1 < 0)
                {
                    //If at the western edge of the map
                    rooms[x, y].exits[3] = Direction.None;
                }
                else
                {
                    //Check for a room to the west of the current room
                    rooms[x, y].exits[3] = (rooms[x - 1, y] != null) ? Direction.West : Direction.None;
                }
            }
        }
    }

    //Return a new position for a room
    Vector2 NewPosition()
    {
        int x = 0; 
        int y = 0;

        Vector2 checkingPos = Vector2.zero;

        //Go through each room while the position is taken
        //and the room is within the boundary of the grid
        do
        {
            //Get a random taken position from the list
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;

            //Randomize the direction the room's exit is going to be
            bool upDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);

            //North or South exit
            if (upDown)
            {
                //North
                if (positive)
                {
                    y += 1;
                }
                //South
                else
                {
                    y -= 1;
                }
            }
            //East or West exit
            else
            {
                //East
                if (positive)
                {
                    x += 1;
                }
                //West
                else
                {
                    x -= 1;
                }
            }

            checkingPos = new Vector2(x, y);

        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX ||
        y >= gridSizeY || y < -gridSizeY);

        return checkingPos;
    }

    //Return a selected position for a room
    Vector2 SelectiveNewPosition()
    {
        int index = 0, inc = 0, x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;

        //Go through each room while the position is taken
        //and the room is within the boundary of the grid
        do
        {
            inc = 0;

            //Get a room that only has one neighber
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;

            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);

            //North or South exit
            if (UpDown)
            {
                //North
                if (positive)
                {
                    y += 1;
                }
                //South
                else
                {
                    y -= 1;
                }
            }
            //East or West exit
            else
            {
                //East
                if (positive)
                {
                    x += 1;
                }
                //West
                else
                {
                    x -= 1;
                }
            }

            checkingPos = new Vector2(x, y);

        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX ||
        y >= gridSizeY || y < -gridSizeY);

        //Error
        if(inc >= 100)
        {
            Debug.LogError("Could not find position with one neighbor");
        }

        return checkingPos;
    }

    //Return the nuber of neighbors a room has
    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int neighbors = 0;

        //Checks for a room to the right of the current room
        if (usedPositions.Contains(checkingPos + Vector2.right))
        {
            neighbors++;
        }

        //Checks for a room to the left of the current room
        if (usedPositions.Contains(checkingPos + Vector2.left))
        {
            neighbors++;
        }

        //Checks for a room above the current room
        if (usedPositions.Contains(checkingPos + Vector2.up))
        {
            neighbors++;
        }

        //Checks for a room bellow the current room
        if (usedPositions.Contains(checkingPos + Vector2.down))
        {
            neighbors++;
        }

        return neighbors;
    }

    //Create the room data
	void CreateRooms()
    {
        //Create a new array of rooms
        rooms = new RoomData[gridSizeX * 2, gridSizeY * 2];

        //Assign data to the starting room
        rooms[gridSizeX, gridSizeY] = new RoomData(Vector2.zero, RoomType.Entrance, roomSize);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;


        float randomCompare = 0.2f;

        //Loop through each of the rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {

            //Percentage of rooms left to create
            float randomPercent = i / (((float)numberOfRooms - 1));

            //Decrease the chance to branch out the further in the loop
            randomCompare = Mathf.Lerp(startComparison, endComparison, randomPercent);

            //Get a valid room position
            checkPos = NewPosition();

            //Test a new position
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;

                //Branching out
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;

                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);

                //Error
                if (iterations >= 50)
                {
                    Debug.LogError("Could not create with fewer neighbors than: "
                        + NumberOfNeighbors(checkPos, takenPositions));
                }
            }

            //Finalize the room's position
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] =
                  new RoomData(checkPos, RoomType.None, roomSize);

            takenPositions.Insert(0, checkPos);
        }
    }
}