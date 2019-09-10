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

    private Vector3 rayOrigin;                          //Position of the origin ray
    private float xOffset;                              //Offset of the origin ray on the x-axis 
    private float yOffset;                              //Offset of the origin ray on the y-axis
    
    
    private List<PassengerMovement> passengerMovement;  //List of passengers on a treadmill

    //Holds all of the passengers
    private Dictionary<Transform, CollisionController> passengerDictionary = 
        new Dictionary<Transform, CollisionController>();

    //Used for initialization
    public override void Start()
    {
        base.Start();

        xOffset = transform.localScale.x / 2f;
        yOffset = (transform.localScale.y / 2f) - 0.1f;
        
    }

    //
    private void Update()
    {   
        CalculatePassengerMovement(movementSpeed);

        MovePassengers(false);
    }

    //Move the passengers based on the passed in boolean
    private void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovement)
        {
            //If the passenger is not in the dictionary add them
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform,
                    passenger.transform.GetComponent<CollisionController>());
            }

            //Moves the passenger before the platform moves
            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity,
                    passenger.standingOnPlatform);
            }

        }
    }

    //Calculates the passenger's movement
    private void CalculatePassengerMovement(Vector2 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();
        float rayLength = 0.2f;

        //Loop through each veritcal ray
        for (int i = 0; i < verticalRayCount + 1; i++)
        {
            rayOrigin = transform.position + (transform.right * -xOffset) +
                (transform.up * yOffset);
            rayOrigin += transform.right * (i * verticalRaySpacing);

            //Check for passengers above the treadmill
            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, transform.up, rayLength, passengerMask);

            Debug.DrawRay(rayOrigin, transform.up * rayLength, Color.magenta);


            for (int j = 0; j < hits.Length; j++)
            {
                //Check if a passenger is on the treadmill
                if (hits[j].distance != 0)
                {
                    //Add new passengers to the hash set
                    if (!movedPassengers.Contains(hits[j].transform))
                    {
                        movedPassengers.Add(hits[j].transform);

                        float pushX = velocity.x * Time.deltaTime;

                        passengerMovement.Add(new PassengerMovement(hits[j].transform,
                            new Vector2(pushX, 0), true, false));
                    }
                }
            }
        }

        /* Need to change how wall sliding works for this to have an effect
        //Loop through each vertical ray
        for (int i = 0; i < horizontalRayCount; i++)
        {
            rayOrigin = transform.position + (transform.right * -xOffset) +
               (transform.up * yOffset);
            rayOrigin += -transform.up * (horizontalRaySpacing * i);

            //Check for passengers to the right of the treadmill
            RaycastHit hit;

            Debug.DrawRay(rayOrigin, -transform.right * rayLength, Color.green);

            //Did the raycast hit anything
            if (Physics.Raycast(rayOrigin, -transform.right, out hit, rayLength, passengerMask))
            {
                //If the passenger is not in the hash set add them and move them
                if (!movedPassengers.Contains(hit.transform))
                {
                    movedPassengers.Add(hit.transform);

                    float pushY = velocity.y * Time.deltaTime;

                    passengerMovement.Add(new PassengerMovement(hit.transform,
                        new Vector3(0, pushY),false,false));
                }
            }
        }*/
    }
}