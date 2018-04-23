using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private BoxCollider boxCollider;
    private CameraController cameraController;

	//Use this for initialization
	void Start()
	{
        boxCollider = transform. GetComponent<BoxCollider>();
        cameraController = Camera.main.GetComponent<CameraController>();
	}

    //When something enters the room
    private void OnTriggerEnter(Collider other)
    {
        //The player enters the room
        if(other.tag == "Player")
        {
            Debug.Log("Player has entered the room: " + transform.name);
            cameraController.SetCameraBoundary(boxCollider  );
           

            //Activate enemys
        }
    }

    //When something exits the room
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            
            //Deactavate enemys
        }
    }

    //Update is called once per frame
    void Update()
	{
		
	}
}
