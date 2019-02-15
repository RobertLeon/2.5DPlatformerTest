//Created by Robert Bryant
//
//Handles the activation of switches
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {

    public bool switchState;                    //State of the switch

    private Animator animator;                  //Reference to the Animator component
    private InputManager inputManager;          //Reference to the InputManager Script    
    private float timer = 0f;                   //Timer for resetting the switch
    private float switchTimer;                  //Time the switch is active
    private bool switchActivated = false;       //Check for the switch being active
    private bool noTimer = false;               //Check if there is a timer on this switch
    

	// Use this for initialization
	public virtual void Start ()
    {
        animator = GetComponent<Animator>();
        inputManager = FindObjectOfType<InputManager>();

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
       
    //Handles the animation of the switch
    private IEnumerator SwitchAnimation()
    {
        animator.SetBool("LeverState", switchState);
        yield return new WaitForSeconds(1f);

        timer = switchTimer;                
    }

    //Detects the player and checks for player input
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Check for the player
        if(collision.tag == "Player")
        {
            //Check for player input
            if ((inputManager.GetKeyDown("Interact") || inputManager.GetButtonDown("Interact")) && !switchActivated)
            {                
                switchState = !switchState;
                switchActivated = true;
                StartCoroutine(SwitchAnimation());
            }
        }
    }


}
