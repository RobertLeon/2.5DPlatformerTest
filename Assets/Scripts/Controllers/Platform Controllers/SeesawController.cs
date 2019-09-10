//Created by Robert Bryant
//
//Handles the rotatiton of seesaw platforms
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawController : PlatformController
{
    public float rotationSpeed = 5f;            //Speed the platform rotates when active
   
    private bool rotateLeft = false;            //Rotating right
    private bool rotateRight = false;           //Rotating left
    private float xOffset;                      //Offset for the raycast location on the x-axis
    private float yOffset;                      //Offset for the raycast location on the y-axis
    private Vector3 rayOrigin;                  //Origin of the raycast
    private List<PassengerMovement> passengerMovement;  //List of passengers

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //Using this to get access to verticalRayCount
        UpdateRaycastOrigins();

        //Assigning the x and y-offset for the platform based on it's scale
        xOffset = transform.localScale.x / 2f;
        yOffset = (transform.localScale.y / 2f)-0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotates the platform
        RotatePlatform();

        //Moving passengers after the platform moves
        MovePassengers(false);
    }

    //Rotates the platform based on the position of passenger
    private void RotatePlatform()
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();
        List<bool> left = new List<bool>();
        List<bool> right = new List<bool>();

        float rayLength = .5f;

        //Check for passengers on the platform
        for(int i = 0; i < verticalRayCount; i++)
        {
            rayOrigin = transform.position + (transform.right * -xOffset) +
                (transform.up * yOffset);
            rayOrigin += transform.right * (i * verticalRaySpacing);

            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, transform.up, rayLength, passengerMask);

            //Loop through each ray
            for (int j = 0; j < hits.Length; j++)
            {
                //Check for passengers on the left side of the platform
                if (i < (verticalRayCount / 2f))
                {
                    Debug.DrawRay(rayOrigin, transform.up * rayLength, Color.magenta);

                    if (hits[j].distance != 0)
                    {
                        //If the passenger is not on the hash set add and move them
                        if (!movedPassengers.Contains(hits[j].transform))
                        {
                            movedPassengers.Add(hits[j].transform);

                            passengerMovement.Add(new PassengerMovement(hits[j].transform,
                                Vector3.zero, true, false));
                        }

                        left.Add(true);

                    }
                    else
                    {
                        left.Add(false);
                    }
                }
                //Check for passengers on the right side of the platform
                else
                {
                    Debug.DrawRay(rayOrigin, transform.up * rayLength, Color.green);

                    if (hits[j].distance != 0)
                    {
                        //If the passenger is not on the hash set add and move them
                        if (!movedPassengers.Contains(hits[j].transform))
                        {
                            movedPassengers.Add(hits[j].transform);

                            passengerMovement.Add(new PassengerMovement(hits[j].transform,
                                Vector3.zero, true, false));
                        }

                        right.Add(true);
                    }
                    else
                    {
                        right.Add(false);
                    }
                }
            }
        }

        //Check if there is someting on one side of the platform
        rotateLeft = left.Contains(true);
        rotateRight = right.Contains(true);

        //Check the platforms rotation on the z-axis
        if (Mathf.RoundToInt(transform.eulerAngles.z) == 0 ||
            (transform.eulerAngles.z > 300 && transform.eulerAngles.z < 360) ||
            (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 60)) 
        {
            
            if (rotateLeft)
            {
                //Lower the left side of the platform
                transform.Rotate(new Vector3(0f, 0f, rotationSpeed) * Time.deltaTime);
            }

            if (rotateRight)
            {
                //Lower the right side of the platform
                transform.Rotate(new Vector3(0f, 0f, -rotationSpeed) * Time.deltaTime);
            }
        }
        //If the platform has been lowered on the left allow it to be lowered on the right side
        else if (transform.localEulerAngles.z > 60 && transform.localEulerAngles.z < 70 && rotateRight)
        {
            transform.Rotate(new Vector3(0f, 0f, -rotationSpeed) * Time.deltaTime);
        }
        //If the platform has been lowered on the right allow it to be lowered on the left side
        else if (transform.localEulerAngles.z < 300 && transform.localEulerAngles.z > 290 && rotateLeft)
        {
            transform.Rotate(new Vector3(0f, 0f, rotationSpeed) * Time.deltaTime);
        }
        
        //If the platform is not rotating reset
        if(!rotateLeft && !rotateRight)
        {
            if(transform.eulerAngles.z > 0 && transform.eulerAngles.z <= 65)
            {
                transform.Rotate(new Vector3(0f, 0f, -rotationSpeed*1.5f) * Time.deltaTime);
            }

            if(transform.eulerAngles.z >= 295 && transform.eulerAngles.z < 360)
            {
                transform.Rotate(new Vector3(0f, 0f, rotationSpeed*1.5f) * Time.deltaTime);
            }
        }
    }        
}
