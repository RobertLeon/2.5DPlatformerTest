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

        StartCoroutine(SwitchAnimation());
        switchActive = false;
        noTimer = switchTimer == 0 ? true : false;
	}

    // Update is called once per frame
    void Update()
    {
        //Check if the switch has been activated
        if (switchActive)
        {
            //Check for a timer and if the timer is set
            if (!noTimer && timer > 0)
            {
                timer -= Time.deltaTime;                
            }
            //Resets the timer and the switch state
            else if (timer <= 0 && switchActive)
            {
                timer = 0;
                switchState = false;
                switchActive = false;
                StartCoroutine(SwitchAnimation());
            }
        }
    }
       
    //Handles the animation of the switch
    private IEnumerator SwitchAnimation()
    {
        animator.SetBool("LeverState", switchState);
        yield return new WaitForSeconds(1f);
        
        if(!noTimer && !switchActive)
        {
            timer = switchTimer;
        }        
    }

    //Detects the player and checks for player input
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Check for the player
        if(collision.tag == "Player")
        {
            Debug.Log("Player Detected");
            //Check for player input
            if ((inputManager.GetKeyDown("Interact") || inputManager.GetButtonDown("Interact")) && !switchActive)
            {                
                switchState = !switchState;
                switchActive = true;
                Debug.Log("Input detected " + switchState);
                StartCoroutine(SwitchAnimation());
            }
        }
    }


}
