//Created by Robert Bryant
//
//Handles the movement of entities in water
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : RaycastController {

    public float waterCurrent;                          //Force of the water on the x-axis
    [Range(0.75f, 0.95f)]
    public float surfaceLevel;                          //Level the water effects entities
    public LayerMask passengerMask;                     //Which layer the passenger is on

    
    private CollisionController[] collisionControllers; //References to the CollisionController script
    private PlayerController playerController;          //Reference to the PlayerController script
    private List<PassengerMovement> passengerMovement;  //List of passengers on a treadmill

    //Holds all of the passengers
    private Dictionary<Transform, CollisionController> passengerDictionary =
        new Dictionary<Transform, CollisionController>();

    //Passenger data
    private struct PassengerMovement
    {
        public Transform transform;     //Passenger object
        public Vector2 velocity;        //Movement of the passenger

        //Constructor
        public PassengerMovement(Transform _transform, Vector2 _velocity)
        {
            transform = _transform;
            velocity = _velocity;
        }
    }

    // Use this for initialization
    public override void Start ()
    {
        base.Start();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int x = enemies.Length + 2;

        collisionControllers = new CollisionController[x];
        
        collisionControllers[0] = player.GetComponent<CollisionController>();
        playerController = player.GetComponent<PlayerController>();
	}

    //
    private void Update()
    {
        UpdateRaycastOrigins();
        CalculatePassengerMovement();
        MovePassengers();
    }

    //Move the passengers based on the passed in boolean
    private void MovePassengers()
    {
        //Loop through each passenger
        foreach (PassengerMovement passenger in passengerMovement)
        {
            //If the passenger is not in the dictionary add them
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform,
                    passenger.transform.GetComponent<CollisionController>());
            }

            passengerDictionary[passenger.transform].Move(passenger.velocity, true);
        }
    }

    //Calculates the passenger's movement
    private void CalculatePassengerMovement()
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();
        float rayLength = boxCollider.bounds.max.y * surfaceLevel;

        //Loop through each veritcal ray
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);

            Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);

            //Check for passengers above the treadmill
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

            //Check if a passenger is on the treadmill
            if (hit)
            {
                //Add new passengers to the hash set
                if (!movedPassengers.Contains(hit.transform))
                {
                    movedPassengers.Add(hit.transform);

                    float pushX = waterCurrent * Time.deltaTime;
                   
                    passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, 0)));
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collisionControllers[0].collisions.inWater = true;            
            playerController.UpdateMovement();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collisionControllers[0].collisions.inWater = false;            
            playerController.UpdateMovement();
        }
    }

}
