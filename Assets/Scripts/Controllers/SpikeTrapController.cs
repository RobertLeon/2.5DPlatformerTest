//Created by Robert Bryant
//
//Handles the activation of traps
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : RaycastController
{
    public float activationTime = 0.5f;     //Time it takes for the trap to become active
    public Transform trap;                  //The trap
    public LayerMask activationMask;        //Entities that will trigger the trap
    [HideInInspector]
    public bool trapActive = false;         //Is the trap active

    private Vector3 trapOnPosition;         //Position of the trap when it is active
    private Vector3 trapOffPosition;        //Position of the trap when it is not active

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //Set the on and off position of the trap
        trapOffPosition = trap.position;
        trapOnPosition = trapOffPosition + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRaycastOrigins();

        DetectTrigger();
    }

    private void DetectTrigger()
    {
        float rayLength = 2 * skinWidth;

        //Loop through each vertical ray
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);

            RaycastHit hit;

            //Did the raycast hit anything
            if (Physics.Raycast(rayOrigin, Vector2.up, out hit, rayLength, activationMask) && hit.distance != 0)
            {
                //Activate the trap
                if ((hit.transform.tag == "Player" || hit.transform.tag == "Enemy")&& !trapActive)
                {
                    trapActive = true;
                    StartCoroutine(ActivateTrap());
                }
            }
        }
    }


    //Activation of the trap
    private IEnumerator ActivateTrap()
    {
        //Wait 
        yield return new WaitForSeconds(activationTime);

        //Set the trap to the on position
        trap.transform.position = trapOnPosition;
         
        //Reset the trap
        StartCoroutine(ResetTrap());
    }

    //Resets the trap
    private IEnumerator ResetTrap()
    {
        yield return new WaitForSeconds(1f);

        //Reset the position of the trap
        trap.transform.position = trapOffPosition;
        trapActive = false;
    }
}
