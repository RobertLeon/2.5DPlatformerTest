//Created by Robert Bryant
//Based off of a tutorial by: Sebastian Lague
//https://www.youtube.com/watch?v=COckHIIO8vk&list=PLFt_AvWsXl0f4c56CbvYi038zmCmoZ4CQ&index=8&t=0s
//Handles the player's animation based on inputs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public float wallSlideOffset = 90f;
    public float slideOffsetR = 145f;
    public float slideOffsetL = 200f;

    private const float animationSmoothTime = .1f;
    private Animator anim;
    private CollisionController collision;
    private PlayerController playerController;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        collision = GetComponent<CollisionController>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent;
        Vector2 playerVelocity = playerController.GetVelocity();
        float maxMoveSpeed = playerController.GetMovementSpeed();

        //Set the angle of the player graphics for the walk animation
        anim.transform.eulerAngles = new Vector3(0, wallSlideOffset * collision.collisions.faceDir);

        //Movement based on the player input
        if (playerVelocity.x != 0)
        {
            speedPercent = Mathf.Abs(playerVelocity.x) / maxMoveSpeed;
        }
        else
        {
            speedPercent = 0;
        }

        //Slide angle offset
        if (collision.collisions.sliding)
        {
            if (collision.collisions.faceDir == 1)
            {
                anim.transform.eulerAngles = new Vector3(0, slideOffsetR);
            }
            else
            {
                anim.transform.eulerAngles = new Vector3(0, slideOffsetL);
            }
        } 
        
        //Wall slide angle offset
        if((collision.collisions.left && !collision.collisions.below) || 
            (collision.collisions.right && !collision.collisions.below))
        {
            anim.transform.eulerAngles = new Vector3(0, wallSlideOffset * -collision.collisions.faceDir);            
            anim.SetBool("IsWallSliding", true);
        }
        else
        {
            anim.SetBool("IsWallSliding", false);           
        }

        //Mirror animation
        anim.SetBool("FaceDir", collision.collisions.faceDir == 1);

        //Sliding animation
        anim.SetBool("IsSliding", collision.collisions.sliding);

        //Jump animation
        anim.SetBool("IsJumping", playerController.isJumping);

        //Falling animation
        anim.SetBool("IsFalling", !collision.collisions.below); 

        //Crouching animation
        anim.SetBool("IsCrouching", playerController.isCrouching);

        //Walk and rin animation
        anim.SetFloat("SpeedPercent", speedPercent, animationSmoothTime, Time.deltaTime);        
    }
}
