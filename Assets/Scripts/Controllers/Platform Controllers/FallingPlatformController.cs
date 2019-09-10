//Created by Robert Bryant
//
//Handles the animation and movement of falling platforms
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController : PlatformController
{
    public float resetTime;                     //Time to reset the platform
    public Color warningColor;                  //Color the platform changes when it is about to fall

    private Animator animator;                  //Reference to the animator component
    private Renderer blockRenderer;             //Reference to the renderer on the platform object
    private Color startColor;                   //Starting color of the platform
    private bool isFalling = false;             //Is the platform falling   
    private float fallTimer;                    //Timer for the object to fall
    private Vector3 startPosition;              //Starting position of the platform
    private Vector3 velocity;                   //Velocty of the falling platfrom
   

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

        fallTimer = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRaycastOrigins();

        //Platform is falling
        if (isFalling)
        {
            //Set the velocity of the platform
            velocity = Vector3.down * platformSpeed * Time.deltaTime;
            
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
        if (DetectPassengers(passengerMask))
        {
            animator.SetBool("Wiggle", true);
            fallTimer -= Time.deltaTime;
            blockRenderer.material.color = warningColor;
        }
        //Reset the timer and stop the animation
        else
        {
            animator.SetBool("Wiggle", false);
            fallTimer = waitTime;
            blockRenderer.material.color = startColor;
        }

        //Set the platform to fall and start the reset process
        if (fallTimer <= 0)
        {
            fallTimer = waitTime;
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
        transform.position = startPosition;
    }
}
