using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;                 //Detect which object layer to collide with
    public const float skinWidth =0.06f;            //Width on the object where the raycasts
    [HideInInspector]                               //start
    public int horizontalRayCount;                  //Amount of rays being drawn horizontally
    [HideInInspector]
    public int verticalRayCount;                    //Amount of rays being drawn vertically
    [HideInInspector]
    public float horizontalRaySpacing;              //Space inbetween the rays being drawn
    [HideInInspector]
    public float verticalRaySpacing;                //Space inbetweeen the rays being drawn
    [HideInInspector]
    public BoxCollider boxCollider;                 //Box Collider Object
    public RaycastOrigins raycastOrigins;           //Origin of the raycast

    private const float dstBestweenRays = .25f;     //Distance between rays  

    //Location of the raycast origin
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
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
        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBestweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBestweenRays);

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
}