//Created by Robert Bryant
//
//Handles the movement of entities on top of a treadmill
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillController : RaycastController
{
    public LayerMask passengerMask;                     //Which layer the passenger is on
    public Vector2 movementSpeed;                       //Speed the treadmill moves a passenger


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

    //Used for initialization
    public override void Start()
    {
        base.Start();
    }

    //
    private void Update()
    {
        UpdateRaycastOrigins();
        CalculatePassengerMovement(movementSpeed);
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

            passengerDictionary[passenger.transform].Move(passenger.velocity, false,true); 
        }
    }

    //Calculates the passenger's movement
    private void CalculatePassengerMovement(Vector2 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();
        float rayLength = skinWidth * 2;

        //Loop through each veritcal ray
        for(int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);

            RaycastHit hit;

            //Check if a passenger is on the treadmill
            if (Physics.Raycast(rayOrigin, Vector2.up, out hit, rayLength, passengerMask))
            {
                //Add new passengers to the hash set
                if (!movedPassengers.Contains(hit.transform))
                {
                    movedPassengers.Add(hit.transform);

                    float pushX = velocity.x * Time.deltaTime;

                    passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, 0)));
                }
            }
        }
    }
}