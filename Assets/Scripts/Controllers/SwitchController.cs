//Created by Robert Bryant
//
//Handles the activation of switches
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {

    public float switchTimer;               //Time for the switch to reset
    public bool switchState;                //State of the switch

    private Animator animator;              //Reference to the Animator component
    private InputManager inputManager;      //Reference to the InputManager Script
    private float timer = 0;                //Timer for resetting the switch
    private bool switchActive = false;      //Check for the switch being active
    private bool noTimer;                   //Check if the switch has a timer

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        inputManager = FindObjectOfType<InputManager>();
        noTimer = switchTimer == 0 ? true : false;        
	}

    // Update is called once per frame
    void Update()
    {
        //Check if there switch has a timer and has been activated
        if(switchActive && !noTimer)
        {
            timer = switchTimer;
            switchActive = false;
        }

        if (!noTimer)
        {
            //Reduce the timer
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            //Reset the switch
            else if (timer <= 0)
            {
                StartCoroutine(SetSwitchToOff());
            }
        }
    }

    //Sets the switch to an off state
    private IEnumerator SetSwitchToOff()
    {
        switchState = false;
        timer = 0;
        animator.SetBool("Lever", switchState);
        yield return new WaitForSeconds(animator.playbackTime);
    }

    //Handles the state and animation of the switches
    private IEnumerator HandleSwitchStates()
    {
        //Check for a timer
        if (noTimer)
        {
            //Set the switch's state to the opposite
            if (switchState)
            {
                switchState = false;
                animator.SetBool("Lever", switchState);
                yield return new WaitForSeconds(animator.playbackTime);
            }
            else
            {
                switchState = true;
                animator.SetBool("Lever", switchState);
                yield return new WaitForSeconds(animator.playbackTime);
            }
        }
        else
        {
            //Check if the switch is off
            if(!switchState)
            {
                switchState = true;
                animator.SetBool("Lever", switchState);
                switchActive = true;
                yield return new WaitForSeconds(animator.playbackTime);
            }
        }
    }

    //
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Check for the player
        if(collision.tag == "Player")
        {
            //Check for player input
            if (inputManager.GetKeyDown("Interact") || inputManager.GetButtonDown("Interact"))
            {
                StartCoroutine(HandleSwitchStates());
            }
        }
    }


}
