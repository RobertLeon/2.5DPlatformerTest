//Created by Robert Bryant
//
//Handles the animation and movement of falling platforms
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallingPlatformController : RaycastController
{
    public LayerMask activationMask;            //Layer mask for what causes the platform to fall
    public float standingTime;                  //Time to stand on the platform before it falls
    public float fallSpeed;                     //Speed of the falling platform
    public float resetTime;                     //Time to reset the platform
    public Color warningColor;                  //Color the platform changes when it is about to fall

    private Animator animator;                  //Reference to the animator component
    private Renderer blockRenderer;             //Reference to the renderer on the platform object
    private Color startColor;                   //Starting color of the platform
    private bool isFalling = false;             //Is the platform falling
    private bool onPlatform = false;            //Is something on the platform
    private float fallTimer;                    //Timer for the object to fall
    private Vector3 startPosition;              //Starting position of the platform
    private Vector3 velocity;                   //Velocty of the falling platfrom
    private List<PassengerMovement> passengerMovement;  //List of passengers

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


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //Get the references to the animator and renderer components
        animator = GetComponentInChildren<Animator>();
        blockRenderer = GetComponentInChildren<Renderer>();

        //Set the start position and start color
        startPosition = transform.position;
        startColor = blockRenderer.material.color;

        fallTimer = standingTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRaycastOrigins();

        //Platform is falling
        if (isFalling)
        {
            //Set the velocity of the platform
            velocity = Vector3.down * fallSpeed * Time.deltaTime;
            onPlatform = false;
        }
        else
        {
            velocity = Vector3.zero;
        }

        //Calculate the passenger's movement
        CalculatePassengerMovement(velocity);

        //Move the passengers and the platform
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);


        //Start the timer and animate the block
        if (onPlatform)
        {
            animator.SetBool("Wiggle", true);
            fallTimer -= Time.deltaTime;
            blockRenderer.material.color = warningColor;
        }
        //Reset the timer and stop the animation
        else
        {
            animator.SetBool("Wiggle", false);
            fallTimer = standingTime;
            blockRenderer.material.color = startColor;
        }

        //Set the platform to fall and start the reset process
        if (fallTimer <= 0)
        {
            fallTimer = standingTime;
            isFalling = true;
            StartCoroutine(ResetPlatform());
        }

    }

    //Reset the falling platform
    private IEnumerator ResetPlatform()
    {
        //Time to wait before reseting the platform
        yield return new WaitForSeconds(resetTime);

        //Reset the flags and position of the platform
        isFalling = false;
        onPlatform = false;
        transform.position = startPosition;
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

                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.green);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, activationMask);

                //Did the Raycast hit anything
                if (hit && hit.distance != 0)
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
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            //Loop through each vertical ray
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.green);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, activationMask);

                //Did the raycast hit anything
                if (hit && hit.distance != 0)
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
            float rayLength = skinWidth * 2;

            //Loop through each vertical ray
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);

                Debug.DrawRay(rayOrigin, Vector2.up, Color.green);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, activationMask);

                //Did the raycast hit anything
                if (hit && hit.distance != 0)
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

        //Check if there is something standing on the platform
        if (velocity == Vector3.zero)
        {
            float rayLength = skinWidth * 2;

            List<bool> standingOnPlatform = new List<bool>();

            //Loop through each vertical ray
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, activationMask);

                //Add the results of the raycast to the list
                standingOnPlatform.Add(hit);          
            }

            //Something is on the platform if the list contains at least one true value
            onPlatform = standingOnPlatform.Contains(true);
        }
    }
}
