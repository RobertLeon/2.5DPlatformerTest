//Created by Robert Bryant
//
//Handles platforms that move and reset after reaching their destination
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetingPlatformController : PlatformController
{
    public float resetTime;                             //Amount of time before the platform resets
   
    private float waitTimer;                            //Timer for activation
    private bool startPlatform = false;                 //Starts the platform's movement
    private bool reseting = false;                      //Check if the platform is resetting
    private List<PassengerMovement> passengerMovement;  //List of passengers

  
    // Start is called before the first frame update
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

        //If a passenger is on the platform count down the timer
        if (DetectPassengers(passengerMask) && waitTimer > 0 && !startPlatform && ! reseting)
        {
            waitTimer -= Time.deltaTime;

            //Activate the platform's movement and reset the timer
            if (waitTimer <= 0)
            {
                waitTimer = 0;
                startPlatform = true;
            }
        }
        //Set the timer for the platform
        else if (DetectPassengers(passengerMask))
        {
            waitTimer = waitTime;
        }
        //Reset the timer for starting the platform
        else
        {
            waitTimer = 0;
        }
        
        Vector3 velocity;

        //Move the platform
        if (startPlatform)
        {
            velocity = CalculatePlatformMovement();
        }
        else
        {            
            velocity = Vector3.zero;
        }


        CalculatePassengerMovement(velocity);

        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

   
    //Calculate the platform's movement
    private Vector3 CalculatePlatformMovement()
    {
        //Calculate the distance between waypoints
        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex],
            globalWaypoints[toWaypointIndex]);       

        //Calculate the easement of the platform
        percentBetweenWaypoints += Time.deltaTime * (platformSpeed / distanceBetweenWaypoints);
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        
        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex],
            globalWaypoints[toWaypointIndex],percentBetweenWaypoints);


        //Reset the timer between platform movement
        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            //Check if the platform is at the end
            if (fromWaypointIndex >= globalWaypoints.Length - 1)
            {
                startPlatform = false;
                fromWaypointIndex = 0;
                StartCoroutine(ResetPlatform(resetTime));
                return Vector3.zero;
            }
        }

        return newPos - transform.position;
    }

    //Resets the platform to the starting position
    private IEnumerator ResetPlatform(float time)
    {
        reseting = true;
        yield return new WaitForSeconds(time);
        MovePassengers(false, reseting);
        transform.position = globalWaypoints[0];
        reseting = false;
    }    
}
