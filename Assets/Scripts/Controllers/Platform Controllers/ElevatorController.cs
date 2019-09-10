//Created by Robert Bryant
//
//Handles the movement of elevator platforms
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : PlatformController
{   
    public LayerMask activationMask;                    //Layer mask for activating the elevator
   
    private Vector2 directionalInput;
   
    private int nextWaypoint = 0;                       //Index of the platform
    private int currentWaypoint = 0;                    //Current waypoint
    private float distanceBetweenWaypoints;             //Distance between waypoints   
    private bool isMoving = false;                      //Is the elevator moving?
    private Renderer rend;                              //Change color for debug use

    private void OnEnable()
    {
        PlayerInput.MovementInput += DirectionalInput;
    }

    private void OnDisable()
    {
        PlayerInput.MovementInput -= DirectionalInput;
    }

    //Use this for initialization
    public override void Start()
	{
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        rend = GetComponent<Renderer>();

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

        //The player moves the elevator up
        if (directionalInput.y == 1 && 
            currentWaypoint < localWaypoints.Length - 1 && !isMoving &&
            DetectPassengers(activationMask))
        {
            nextWaypoint += 1;
            isMoving = true;
        }

        //The player moves the elevator down
        if (directionalInput.y == -1 &&
            currentWaypoint > 0 && !isMoving && DetectPassengers(activationMask))
        {
            nextWaypoint -= 1;
            isMoving = true;
        }
         
        //Move the elevator and the passengers
        if (isMoving)
        {            
            Vector3 velocity = CalculatePlatformMovement(currentWaypoint, nextWaypoint);
            CalculatePassengerMovement(velocity);
            MovePassengers(true);
            transform.Translate(velocity);
            MovePassengers(false);
            
            rend.material.color = Color.red;
        }
        else
        {
            rend.material.color = Color.white;
        }
    }
    
    //Set the Waypoint for the elevator
    public void SetWaypoint(int waypoint)
    {
        //Error if the waypoint given is out of bounds
        if(waypoint >= localWaypoints.Length || waypoint < 0)
        {
            Debug.LogError("Waypoint:" + waypoint + " not found on elevator: " + transform.name);
        }
        //Move the elevator to the specified waypoint when it is not in use
        else if(!isMoving)
        {
            nextWaypoint = waypoint;
            isMoving = true;
        }
    }

    //Get the player's input
    private void DirectionalInput(Vector2 input)
    {
        directionalInput = input;
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
        distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[current],
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
