using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAssigner : MonoBehaviour
{
    public RoomInstance[] roomPrefabs;          //Prefabs of the different rooms to select from
    public Vector2 roomSize;                    //Size of the room

    private RoomInstance selectedRoom;          //Selected room to instantiate
    private int roomNumber = 1;                 //Number of rooms created

    //Assign the Prefab room to the world
    public void Assign(RoomData[,] rooms)
    {
        //Loop through each room that has been passed in
        foreach (RoomData room in rooms)
        {
            //If the room is null go to the next room
            if (room == null)
            {
                continue;
            }

            //Get a random room to instantiate
            GetRoom(room);
            
            //Set the position of the room
            Vector3 position = new Vector3(room.gridPos.x * roomSize.x, room.gridPos.y * roomSize.y, 0);
           
            //Instantiate the room in the game's world space
            selectedRoom = Instantiate(selectedRoom, position, Quaternion.identity,transform).GetComponent<RoomInstance>();
           
            //Set up the room's data
            selectedRoom.SetUp(room.gridPos, room.roomType);

            //Change the room's name in the scene
            selectedRoom.name = selectedRoom.roomType + " " + roomNumber;

            roomNumber++;
        }
    }

    //Gets a random room with from the prefabs that matches the given room data
    private void GetRoom(RoomData room)
    {
        selectedRoom = null;
        List<RoomInstance> randomRoom = new List<RoomInstance>();

        //Loop through the rooms
        for (int i = 0; i < roomPrefabs.Length; i++)
        {
            //Check for matching room types
            if(roomPrefabs[i].roomType == room.roomType)
            {

            }
            //Check if the room have the same amount of exits
            if (roomPrefabs[i].ExitCount() == room.ExitCount())
            {
                //If all the exits match add them to the list
                if (roomPrefabs[i].exits[0] == room.exits[0] &&
                    roomPrefabs[i].exits[1] == room.exits[1] &&
                    roomPrefabs[i].exits[2] == room.exits[2] &&
                    roomPrefabs[i].exits[3] == room.exits[3])
                {
                    randomRoom.Add(roomPrefabs[i]);
                }
            }
        }

        //Pick a random room from the list
        selectedRoom = randomRoom[Random.Range(0, randomRoom.Count)];
    }
}