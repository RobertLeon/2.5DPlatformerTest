using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadmillController : RaycastController
{
    public LayerMask passengerMask;
    public Vector2 movementSpeed;

    private List<PassengerMovement> passengerMovement;
    private Dictionary<Transform, CollisionController> passengerDictionary = 
        new Dictionary<Transform, CollisionController>();

    private struct PassengerMovement
    {
        public Transform transform;
        public Vector2 velocity;

        public PassengerMovement(Transform _transform, Vector2 _velocity)
        {
            transform = _transform;
            velocity = _velocity;
        }
    }


    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        CalculatePassengerMovement(movementSpeed);
        MovePassengers();
    }

    //Move the passengers based on the passed in boolean
    private void MovePassengers()
    {
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

    private void CalculatePassengerMovement(Vector2 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();
        float rayLength = skinWidth * 2;

        for(int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);

            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector2.up, out hit, rayLength, passengerMask))
            {
                if (!movedPassengers.Contains(hit.transform))
                {
                    movedPassengers.Add(hit.transform);

                    float pushX = velocity.x * Time.deltaTime;
                    float pushY = velocity.y * Time.deltaTime;

                    passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, 0)));
                }
            }
        }
    }
}