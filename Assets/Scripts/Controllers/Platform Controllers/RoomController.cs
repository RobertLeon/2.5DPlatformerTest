//Created by Robert Bryant
//
//Handles the activation of the camera boundary and enemy logic
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomController : MonoBehaviour
{
    public RoomData roomData;                           //Data of this room
    public EnemyData[] enemyData;                       //

    private BoxCollider boxCollider;                    //Reference to the Box Collider

    public delegate void SetBoundary(BoxCollider box);
    public static event SetBoundary CameraBoundary;
 

    [System.Serializable]
    public struct EnemyData
    {
        public GameObject enemyPrefab;
        public Vector3[] spawnLocations;
        public int poolSize;        
    }


    //Use this for initialization
    void Start()
	{
        //Get the box collider and camera controller scripts
        boxCollider = transform.GetComponent<BoxCollider>();

        //Force the box collider to be a trigger
        boxCollider.isTrigger = true;
        
        for(int i = 0; i < enemyData.Length; i++)
        {
            GameObject parent = new GameObject(enemyData[i].enemyPrefab.name + " Pool");
            ObjectPoolManager.Instance.CreatePool(parent.transform, enemyData[i].enemyPrefab, enemyData[i].poolSize);
        }

        for(int i = 0; i < enemyData.Length; i++)
        {
            for(int j = 0; j < enemyData[i].spawnLocations.Length; j++)
            {
                ObjectPoolManager.Instance.UsePoolObject(enemyData[i].enemyPrefab,
                    enemyData[i].spawnLocations[j], Quaternion.identity);
            }
        }
	}

    //When something enters the room
    private void OnTriggerEnter(Collider other)
    {
        //The player enters the room
        if (other.tag == "Player")
        {
            //Set the camera boundaries for the current room
            CameraBoundary(boxCollider);

            //Activate enemys
        }
    }

    //When something exits the room
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Deactavate enemys
        }
    }

    //Draws gizmnos in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = roomData.GetRoomColor();
        Gizmos.DrawCube(transform.position, transform.localScale);


        Gizmos.color = Color.red;
        if(enemyData.Length > 0)
        {
            for (int i = 0; i < enemyData.Length; i++)
            {
                if (enemyData[i].spawnLocations.Length > 0)
                {
                    for (int j = 0; j < enemyData[i].spawnLocations.Length; j++)
                    {
                        Gizmos.DrawCube(transform.position + enemyData[i].spawnLocations[j], Vector3.one);
                    }
                }
            }
        }
    }
}
