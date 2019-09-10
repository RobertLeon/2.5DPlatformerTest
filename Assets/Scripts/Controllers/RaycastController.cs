//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/2DPlatformer-Tutorial
//Changes the amount of rays being cast from an object based on size
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;                 //Detect which object layer to collide with
    public const float skinWidth = 0.015f;          //Width on the object where the raycasts starts                               
    [HideInInspector]
    public int horizontalRayCount;                  //Amount of rays being drawn horizontally
    [HideInInspector]
    public int verticalRayCount;                    //Amount of rays being drawn vertically
    [HideInInspector]
    public float horizontalRaySpacing;              //Space inbetween the rays being drawn
    [HideInInspector]
    public float verticalRaySpacing;                //Space inbetweeen the rays being drawn
    [HideInInspector]
    public BoxCollider boxCollider;                 //Box Collider component
    public RaycastOrigins raycastOrigins;           //Origin of the raycast

    private const float dstBetweenRays = 0.05f;     //Distance between rays  

    //Location of the raycast origin
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct PassengerMovement
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
    
    public virtual void Awake()
    {
        //Reference for the Box Collider component
        boxCollider = GetComponent<BoxCollider>();
    }

    //Use this for initialization
	public virtual void Start()
	{        
        CalculateRaySpacing();
	}

    //Recalculate the ray spacing on an object
    public void UpdateRaySpacing()
    {
        CalculateRaySpacing();
    }

    //Calculate the space inbetween ray casts being drawn
    private void CalculateRaySpacing()
    {
        //The area where the rays are being drawn
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);        

        //The height and width of the object
        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        //Calculate the amount of rays being cast
        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        //Calculate the spacing between each ray
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

    }

    //Updating the origin of the raycast
    public void UpdateRaycastOrigins()
    {
        //The area where the rays are being drawn
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        //Location of each corner of the object
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    //Detects passengers on the object
    public bool DetectPassengers(LayerMask activationMask)
    {
        float rayLength = skinWidth * 2;

        List<bool> standingOnPlatform = new List<bool>();

        //Loop through each vertical ray
        for (int i = 0; i < verticalRayCount; i++)
        {
            //Location of the starting ray
            Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);

            //Detect all passengers on this raycast
            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector2.up, rayLength, activationMask);

            //Loop through each raycast
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j].distance != 0)
                {
                    //Add the results of the raycast to the list
                    standingOnPlatform.Add(hits[j].transform);
                }
            }
        }
        
        //Something is on the platform if the list contains at least one true value
        return standingOnPlatform.Contains(true);
    }
}