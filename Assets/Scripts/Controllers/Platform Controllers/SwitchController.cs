//Created by Robert Bryant
//
//Handles the activation of switches
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {

    public bool switchState;                    //State of the switch

    private Transform switchUser;               //The transform of the user
    private Animator animator;                  //Reference to the Animator component        
    private float timer = 0f;                   //Timer for resetting the switch
    private float switchTimer;                  //Time the switch is active
    private bool switchActivated = false;       //Check for the switch being active
    private bool noTimer = false;               //Check if there is a timer on this switch


    private void OnEnable()
    {
        PlayerInput.ActivateObject += ActivateSwitch;
    }

    private void OnDisable()
    {
        PlayerInput.ActivateObject -= ActivateSwitch;
    }

    // Use this for initialization
    public virtual void Start ()
    {
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(SwitchAnimation());
        switchActivated = false;        
	}

    //Set the timer the switch is active
    public void SetTimer(float time)
    {
        switchTimer = time;

        noTimer = switchTimer == 0 ? true : false;
    }

    //Handles the switch timer and reseting the switch states
    public void SwitchCountDown()
    {
        //Check if the switch has been activated
        if (switchActivated)
        {
            //Check for a timer and if the timer is set
            if (!noTimer && timer > 0)
            {
                timer -= Time.deltaTime;                
            }
            //Resets the timer and the switch state
            else if (timer <= 0 && switchActivated)
            {
                timer = 0;
                switchState = false;
                switchActivated = false;
                StartCoroutine(SwitchAnimation());
            }
        }
    }

    private void ActivateSwitch()
    {
        if (switchUser != null)
        {
            switchState = !switchState;
            switchActivated = true;
            StartCoroutine(SwitchAnimation());
        }
    }
       
    //Handles the animation of the switch
    private IEnumerator SwitchAnimation()
    {
        animator.SetBool("LeverState", switchState);
        yield return new WaitForSeconds(1f);

        timer = switchTimer;                
    }

  
    //Detects the player and checks for player input
    private void OnTriggerEnter(Collider other)
    {
        //Check for the player
        if (other.tag == "Player")
        {
            switchUser = other.transform;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            switchUser = null;
        }
    }
}
