//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/2DPlatformer-Tutorial
//Handles the movement of certain platforms

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{
    public LayerMask passengerMask;                     //Layer masks for passengers
    public float platformSpeed;                         //Speed the platform moves
    public float waitTime;                              //Amount of time inbetween movement
    [Range(0,2)]
    public float easeAmount;                            //Smooths movement at the end of a waypoint
    public bool cyclic;                                 //Does the platform cycle    
    public Vector3[] localWaypoints;                    //Waypoints for the platform
    [HideInInspector]
    public Vector3[] globalWaypoints;                   //Waypoints to cycle through
    [HideInInspector]
    public int fromWaypointIndex;                      //Index of the platform
    [HideInInspector]
    public float percentBetweenWaypoints;              //Percentage between 0 and 1
    [HideInInspector]
    public float nextMoveTime;
    //Timer for movement of the platform
    private List<PassengerMovement> passengerMovement;  //List of passengers

    //Holds all the passengers on a platform
    private Dictionary<Transform, CollisionController> passengerDictionary = 
        new Dictionary<Transform, CollisionController>();

    //Use this for initialization
    public override void Start()
    {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];

        //Loop through each waypoint and add them to the array
        for (int i = 0; i < globalWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    //Update is called once per frame
    void Update()
    {
        UpdateRaycastOrigins();

        Vector3 velocity = CalculatePlatformMovement();

        CalculatePassengerMovement(velocity);

        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    //Move the passengers based on the passed in boolean
    public void MovePassengers(bool beforeMovePlatform, bool dropPassengers = false)
    {
        if (passengerMovement != null)
        {
            foreach (PassengerMovement passenger in passengerMovement)
            {
                //Stops moving passengers
                if (dropPassengers)
                {
                    break;
                }

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
    }

    //Calculates the passenger's movement on the platform
    public void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //Platform is moving on the y-axis
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            //Loop through each vertical ray
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);

                RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                //Did the Raycast hit anything
                for (int j = 0; j < hits.Length; j++)
                {
                    if (hits[j].distance != 0)
                    {
                        //If the passenger is not in the hash set add them and move them
                        if (!movedPassengers.Contains(hits[j].transform))
                        {
                            movedPassengers.Add(hits[j].transform);

                            float pushX = (directionY == 1) ? velocity.x : 0;
                            float pushY = velocity.y - (hits[j].distance - skinWidth) * directionY;

                            passengerMovement.Add(new PassengerMovement(hits[j].transform,
                                new Vector3(pushX, pushY), directionY == 1, true));
                        }
                    }
                }
            }
        }

        //Platform is moving on the x-axis pushes objects out of the way
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            //Loop through each horizontal ray
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);

                RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                for (int j = 0; j < hits.Length; j++)
                {
                    //Did the raycast hit anything
                    if (hits[j].distance != 0)
                    {
                        //If the passenger is not in the hash set add them and move them
                        if (!movedPassengers.Contains(hits[j].transform))
                        {
                            movedPassengers.Add(hits[j].transform);

                            float pushX = velocity.x - (hits[j].distance - skinWidth) * directionX;
                            float pushY = -skinWidth;

                            passengerMovement.Add(new PassengerMovement(hits[j].transform,
                                new Vector3(pushX, pushY), false, true));
                        }
                    }
                }
            }
        }

        //Passenger is on top of a platform moving horizontaly or moving downwards
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            //Loop through each vertical ray
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);

                RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector2.up, rayLength, passengerMask);

                for (int j = 0; j < hits.Length; j++)
                {
                    //Did the raycast hit anything
                    if (hits[j].distance != 0)
                    {
                        //If the passenger is not on the hash set add and move them
                        if (!movedPassengers.Contains(hits[j].transform))
                        {
                            movedPassengers.Add(hits[j].transform);

                            float pushX = velocity.x;
                            float pushY = velocity.y;

                            passengerMovement.Add(new PassengerMovement(hits[j].transform,
                                new Vector3(pushX, pushY), true, false));
                        }
                    }
                }
            }
        }
    }

    //Calculate the platform's movement
    private Vector3 CalculatePlatformMovement()
    {
        //Stops the platform
        if(Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        //Calculate the distance between waypoints
        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex],
            globalWaypoints[toWaypointIndex]);

        //Calculate the easement of the platform
        percentBetweenWaypoints += Time.deltaTime * (platformSpeed / distanceBetweenWaypoints);
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easePercent = CalculateEase(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex],
            globalWaypoints[toWaypointIndex], easePercent);

        //Reset the timer between platform movement
        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            //If the platform does not cycle through movement
            if (!cyclic)
            {
                //If the platform is at the last point then it reverses the order
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }
        
        return newPos - transform.position;
    }

    //Calculate the easement of the platform
    private float CalculateEase(float x)
    {
        float a = easeAmount + 1;
        return (Mathf.Pow(x, a) / (Mathf.Pow(x, a) + (Mathf.Pow(1 - x, a))));
    }

    //Draws the position where the platform is going to move
    private void OnDrawGizmos()
    {
        if(localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .5f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWayPointsPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWayPointsPos - Vector3.up * size, globalWayPointsPos + Vector3.up * size);
                Gizmos.DrawLine(globalWayPointsPos - Vector3.left * size, globalWayPointsPos + Vector3.left * size);
            }
        }

        
    }
}