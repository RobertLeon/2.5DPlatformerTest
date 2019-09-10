//Created by Robert Bryant
//
//Handles the movement of entities in water
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : RaycastController {

    public float waterCurrent;                          //Force of the water on the x-axis
    public float bouyancy;
    public float freq;
    public float surfaceLevel;                          //Level the water effects entities
    public LayerMask passengerMask;                     //Which layer the passenger is on
    
    private CollisionController[] collisionControllers; //References to the CollisionController script
    private PlayerController playerController;          //Reference to the PlayerController script
    private List<PassengerMovement> passengerMovement;  //List of passengers on a treadmill

    //Holds all of the passengers
    private Dictionary<Transform, CollisionController> passengerDictionary =
        new Dictionary<Transform, CollisionController>();

    public delegate void Bouancy(bool inWater);
    public static event Bouancy WaterState;

   
    // Use this for initialization
    public override void Start ()
    {
        base.Start();
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
        float rayLength = surfaceLevel;

        //Loop through each veritcal ray
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);

            Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);

            //Check for passengers above the treadmill
            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector2.up, rayLength, passengerMask);

            for (int j = 0; j < hits.Length; j++)
            {
                //Check if a passenger is on the treadmill
                if (hits[j].distance != 0)
                {
                    //Add new passengers to the hash set
                    if (!movedPassengers.Contains(hits[j].transform))
                    {
                        movedPassengers.Add(hits[j].transform);

                        float pushY = bouyancy * Time.deltaTime;
                        float pushX = waterCurrent * Time.deltaTime;

                        passengerMovement.Add(new PassengerMovement(hits[j].transform,
                            new Vector2(pushX, pushY), true, false));
                    }
                }
            }
        }            
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            WaterState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            WaterState(false);
        }
    }
}
