//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/2DPlatformer-Tutorial
//Controls the camera's movement based on the player's movement

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CollisionController cameraTarget;            //Camera's target to follow
    public Vector2 focusAreaSize;                       //Size of the focus area
    public float lookAheadDstX;                         //Distance to look ahead on the x-axis
    public float lookSmoothTimeX;                       //Time to
    public float verticalSmoothTime;                    //Time to smooth vertical movement
    public float verticalOffset;                        //Offset on the y-axis
    public Color gizmoColor;                            //Color for the gizmo in the editor window

    private FocusArea focusArea;                        //Area for the camera to focus on
    private float currentLookAheadX;                    //Current look ahead distance
    private float targetLookAheadX;                     //Target look ahead distance
    private float lookAheadDirX;                        //Direction to look ahead
    private float smoothLookVelocityX;                  //Amount to smooth the camera movement
    private float smoothVelocityY;                      //Amount to move the camera on the y-axis
    private bool lookAheadStopped;                      //Has the look ahead stopped?

    //Structure for the Focus Area
    private struct FocusArea
    {
        public Vector2 center;          //Center of the focus area
        public Vector2 velocity;        //Camera movement
        float left, right;              //Left and right boundary
        float top, bottom;              //Top and bottom boundary

        //Constructor
        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        //Updates the boundary of the focus area
        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;

            //Camera target is moveing ledt
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            //Camera target is moving right
            else if( targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            left += shiftX;
            right += shiftX;

            float shiftY = 0;

            //Camera target is moving down
            if(targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            //Camera target is moving up
            else if(targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }

    private void Start()
    {
        //If there is no camera target
        if (cameraTarget == null)
        {
            //Find the player object and set them as the target
            cameraTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<CollisionController>();

            focusArea = new FocusArea(cameraTarget.boxCollider.bounds, focusAreaSize);
        }
    }

    private void LateUpdate()
    {
        //If the camera target exists
        if (cameraTarget != null)
        {
            focusArea.Update(cameraTarget.boxCollider.bounds);

            Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

            //Set the look ahead direction on the x-axis
            if (focusArea.velocity.x != 0)
            {
                lookAheadDirX = Mathf.Sign(focusArea.velocity.x);

                //If the player and camera are moving in the same direction calculate the target
                //look ahead distance
                if (Mathf.Sign(cameraTarget.playerInput.x) == Mathf.Sign(focusArea.velocity.x) &&
                    cameraTarget.playerInput.x != 0)
                {
                    lookAheadStopped = false;
                    targetLookAheadX = lookAheadDirX * lookAheadDstX;
                }
                //Moves the camera a small amount when moving
                else
                {
                    if (!lookAheadStopped)
                    {
                        lookAheadStopped = true;
                        targetLookAheadX = currentLookAheadX + 
                            (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;
                    }
                }
            }

            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX,
                ref smoothLookVelocityX, lookSmoothTimeX);

            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
            focusPosition += Vector2.right * currentLookAheadX;

            transform.position = (Vector3)focusPosition + Vector3.forward * -10;
        }
    }

    private void OnDrawGizmos()
    {
        gizmoColor.a = 0.5f;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }
}