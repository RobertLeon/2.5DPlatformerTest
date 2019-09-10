//Created by Robert Bryant
//
//Handles 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwitch : SwitchController
{
    public GameObject elevator;                         //Elevator the switch is connected
    public int evelatorLevel;                           //Waypoint level the elevator moves

    private float elevatorTimer = 1f;                   //Timer for the elevator switch
    private ElevatorController elevatorController;      //Reference to the ElevatorController Script

    // Start is called before the first frame update
    public override void Start()
    {
        elevatorController = elevator.GetComponent<ElevatorController>();        
        base.Start();
        SetTimer(elevatorTimer);
    }

    // Update is called once per frame
    public void Update()
    {
        //Set the waypoint for the elevator if the switch is activated
        if(switchState)
        {
            elevatorController.SetWaypoint(evelatorLevel);
        }

        //Timer for the switch
        SwitchCountDown();
    }
}
