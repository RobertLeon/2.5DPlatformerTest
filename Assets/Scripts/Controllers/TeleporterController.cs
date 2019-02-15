//Created by Robert Bryant
//
//Handles the movement caused by teleportation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    public Transform exit;                              //Teleporter's Exit
    public Transform screenEffect;                      //Screen effect to use when teleporting
    public bool requiresInput;                          //Does the teleport require input to function
    public float teleportCoolDown = 1f;                 //Time for the teleport to cool down
    
    private InputManager inputManager;                  //Reference to the Input Manager
    private TeleporterController exitController;        //Reference to the exit teleport controller
    private Animator animator;                          //Reference to the animator component
    private float timer = 0f;                           //Timer for the teleport cool down
    private bool canTeleport = true;                    //Check if the teleporter can be used


    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        exitController = exit.GetComponent<TeleporterController>();
        animator = screenEffect.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            canTeleport = true;
            timer = 0;
        }
        else if(timer > 0)
        {
            timer -= Time.deltaTime;
        }        
    }

    //Disables the teleporter
    public void DisableTeleporter()
    {
        canTeleport = false;
    }

    //Enables the telporter
    public void EnableTeleporter()
    {
        canTeleport = true;
    }

    //Moves the passenger and handles the animation of screen effects
    private IEnumerator Teleport(Transform passenger)
    {
        //Check if the passenger is a player
        if (passenger.tag == "Player")
        {
            animator.SetBool("FadeOut", true);
            SetTeleportCoolDown();
            exitController.SetTeleportCoolDown();

            yield return new WaitForSeconds(0.25f);

            passenger.position = exit.position;
            animator.SetBool("FadeOut", false);
        }
        else
        {
            passenger.position = exit.position;
        }
    }

    //Set the cool down on the teleporter
    public void SetTeleportCoolDown()
    {
        canTeleport = false;
        timer = teleportCoolDown;        
    }

    //
    private void OnTriggerStay2D(Collider2D collision)
    {
        //The player is in the 
        if (collision.tag == "Player")
        {
            //Does the teleporter require input to activate?
            if (requiresInput)
            {
                //Check for the required player input
                if ((inputManager.GetButtonDown("Interact") || inputManager.GetKeyDown("Interact")) && canTeleport)
                {
                    StartCoroutine(Teleport(collision.transform));                    
                }
            }
            //Just teleport the player
            else
            {
                if (canTeleport)
                {
                    StartCoroutine(Teleport(collision.transform));
                }
            }
        }
    }

}
