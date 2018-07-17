//Created by Robert Bryant
//
//Handles the movement of elevator platforms
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController :RaycastController
{
    public LayerMask passengerMask;                     //Layer mask for passengers
    public float platformSpeed;                         //Speed the platform moves
    public float waitTime;                              //Amount of time inbetween movement
    [Range(0, 2)] public float easeAmount;              //Smooths movement at the end of a waypoint  
    public Vector3[] localWaypoints;                    //Waypoints for the platform

    private Vector3[] globalWaypoints;                  //Waypoints to cycle through
    private int nextWaypoint = 0;                       //Index of the platform
    private int currentWaypoint = 0;                    //Current waypoint
    private float percentBetweenWaypoints;              //Percentage betwee 0 and 1
    private float nextMoveTime;                         //Timer for movement of the platform
    private float distanceBetweenWaypoints;             //Distance between waypoints
    private List<PassengerMovement> passengerMovement;  //List of passengers
    private PlayerController playerController;          //Reference to the Player Controller script
    private bool isMoving = false;                      //Is the elevator moving?


    //Holds all the passengers on a platform
    private Dictionary<Transform, CollisionController> passengerDictionary =
        new Dictionary<Transform, CollisionController>();

    //Passenger data
    private struct PassengerMovement
    {
        public Transform transform;         //Passenger object
        public Vector3 velocity;            //Movement of the passenger
        public bool standingOnPlatform;     //Check for standing on the platform
        public bool moveBeforePlatform;     //Moves before the platform does

        //Constructor
        public PassengerMovement(Transform _passenger, Vector3 _velocity,
            bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _passenger;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    //Use this for initialization
    public override void Start()
	{
        base.Start();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

        float rayLength = 2 * skinWidth;

        //Loop through each vertical ray
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);

            RaycastHit hit;

            //Did the raycast hit anything
            if (Physics.Raycast(rayOrigin, Vector2.up, out hit, rayLength, passengerMask) && hit.distance != 0)
            {
                if (hit.transform.tag == "Player")
                {
                    //The player moves the elevator up
                    if (playerController.directionalInput == Vector2.up && currentWaypoint < localWaypoints.Length - 1 && !isMoving)
                    {
                        nextWaypoint += 1;
                        isMoving = true;
                    }

                    //The player moves the elevator down
                    if (playerController.directionalInput == Vector2.down && currentWaypoint > 0 && !isMoving)
                    {
                        nextWaypoint -= 1;
                        isMoving = true;
                    }
                }
            }
        }


        //Move the elevator and the passengers
        if (isMoving)
        {            
            Vector3 velocity = CalculatePlatformMovement(currentWaypoint, nextWaypoint);
            CalculatePassengerMovement(velocity);
            MovePassengers(true);
            transform.Translate(velocity);
            MovePassengers(false);
        }
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
                passengerDictionary[passenger.transform].Move(passenger.velocity, false,
                    passenger.standingOnPlatform);
            }

        }
    }

    //Calculates the passenger's movement on the platform
    private void CalculatePassengerMovement(Vector3 velocity)
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

                RaycastHit hit;

                //Did the Raycast hit anything
                if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, rayLength, passengerMask) && hit.distance != 0)
                {
                    //If the passenger is not in the hash set add them and move them
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform,
                            new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        //Platform is moving on the x-axis pushes objects out of the way
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            //Loop through each vertical ray
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (verticalRaySpacing * i);

                RaycastHit hit;

                //Did the raycast hit anything
                if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, rayLength, passengerMask) && hit.distance != 0)
                {
                    //If the passenger is not in the hash set add them and move them
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform,
                            new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        //Passenger is on top of a platform moving horizontaly or moving downwards
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = 2 * skinWidth;

            //Loop through each vertical ray
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);

                RaycastHit hit;

                //Did the raycast hit anything
                if (Physics.Raycast(rayOrigin, Vector2.up, out hit, rayLength, passengerMask) && hit.distance != 0)
                {
                    //If the passenger is not on the hash set add and move them
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform,
                            new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }


    //Calculate the platform's movement
    private Vector3 CalculatePlatformMovement(int current, int next)
    {
        //Stops the platform
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;  
        }

        //Calculate the distance between waypoints
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[current],
            globalWaypoints[next]);
        
        //Calculate the easement of the platform
        percentBetweenWaypoints += (Time.deltaTime * platformSpeed) / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easePercent = CalculateEase(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[current],
            globalWaypoints[next], easePercent);

        
        //Reset the timer between platform movement
        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            currentWaypoint = next;
            isMoving = false;

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
        if (localWaypoints != null)
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
