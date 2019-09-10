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

    private Transform passenger;
    private InputManager inputManager;                  //Reference to the Input Manager
    private TeleporterController exitController;        //Reference to the exit teleport controller
    private Animator animator;                          //Reference to the animator component
    private float timer = 0f;                           //Timer for the teleport cool down
    private bool teleporterIsActive = true;                    //Check if the teleporter can be used


    private void OnEnable()
    {
        PlayerInput.ActivateObject += Teleport;
    }

    private void OnDisable()
    {
        PlayerInput.ActivateObject -= Teleport;

    }

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
            teleporterIsActive = true;
            timer = 0;
        }
        else if(timer > 0)
        {
            timer -= Time.deltaTime;
        }       
    }

    //Disables the teleporter
    private void DisableTeleporter()
    {
        teleporterIsActive = false;
    }

    //Enables the telporter
    private void EnableTeleporter()
    {
        teleporterIsActive = true;
    }


    private void Teleport()
    {
        if(passenger != null)
        {
            StartCoroutine(TeleportEffect(passenger));
        }
    }

    //Moves the passenger and handles the animation of screen effects
    private IEnumerator TeleportEffect(Transform passenger)
    {
        //Check if the passenger is a player
        if (passenger.tag == "Player")
        {
            animator.SetBool("FadeOut", true);
            SetTeleportCoolDown();
            exitController.SetTeleportCoolDown();

            yield return new WaitForSeconds(0.25f);
            
            passenger.position = exit.position -Vector3.forward;
            animator.SetBool("FadeOut", false);
        }
        else
        {
            passenger.position = exit.position - Vector3.forward;
        }
    }

    //Set the cool down on the teleporter
    public void SetTeleportCoolDown()
    {
        teleporterIsActive = false;
        timer = teleportCoolDown;        
    }

   
    //
    private void OnTriggerStay(Collider other)
    {
        //The player is in the 
        if (other.tag == "Player")
        {
            //Does the teleporter require input to activate?
            if (requiresInput && teleporterIsActive)
            {
                passenger = other.transform;
            }
            //Just teleport the player
            else
            {
                if (teleporterIsActive)
                {
                    passenger = other.transform;
                }
            }
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        passenger = null;
    }


}
