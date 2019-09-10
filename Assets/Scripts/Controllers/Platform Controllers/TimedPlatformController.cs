//Created by Robert Bryant
//
//Handles the activation and deactivation of timed platforms
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatformController : MonoBehaviour
{
    public TimedPlatforms[] platforms;
    public float timeActive = 4f;                       //Time the platform is active
    public float nextActiveTime = 2f;                   //Time before the next platform spawns in

    private int currentOrder = 0;                       //Current order of the platforms
    private int maxOrder = 0;                           //Maximum order of the platforms
    private int nextOrder = 0;                          //Next platform to activate
    [SerializeField]private float[] waitTimes;          //Wait times for the platforms
    [SerializeField]private float[] activeTimes;        //Times the platforms are active
    
    [System.Serializable]
    public struct TimedPlatforms
    {
        public GameObject platforms;    //Platform prefab
        public int activationOrder;     //Order the platform starts
        public float waitTime;          //Time the platform waits to activate
        public float activeTime;        //Time the platform is active
    }

    // Start is called before the first frame update
    void Start()
    {
        //Starts the platforms timers
        StartPlatforms();
    }

    // Update is called once per frame
    void Update()
    {
        //Reset the current order
        if(currentOrder > maxOrder - 1)
        {
            currentOrder = 0;
        }

        //Get the next order of platforms to activate
        nextOrder = (currentOrder + 1) % maxOrder;

        //Loop through each platform
        for (int i = 0; i < platforms.Length; i++)
        {
            //Deactivate the platform
            if (activeTimes[i] > platforms[i].activeTime)
            {
                platforms[i].activeTime = activeTimes[i] = 0f;
                platforms[i].platforms.SetActive(false);
            }

            //Activate the platform
            if (waitTimes[i] > platforms[i].waitTime)
            {
                platforms[i].waitTime = waitTimes[i] = 0f;
                platforms[i].platforms.SetActive(true);
                platforms[i].activeTime = timeActive;
                
                //Loop through each platform for the next in order
                for (int j = 0; j < platforms.Length; j++)
                {
                    //Set the wait time of the next platforms in order
                    if (platforms[j].activationOrder == nextOrder)
                    {
                        platforms[j].waitTime = nextActiveTime;
                    }
                }
                
                //Increase the current order
                currentOrder++;
            }

            //Wait Timer
            if (platforms[i].waitTime != 0f)
            {
                waitTimes[i] += Time.deltaTime;
            }
            else
            {
                waitTimes[i] = 0f;
            }

            //Activation Timer
            if (platforms[i].activeTime != 0f)
            {
                activeTimes[i] += Time.deltaTime;
            }
            else
            {
                activeTimes[i] = 0f;
            }
        }
    }

    //Initializes the platforms timers
    void StartPlatforms()
    {
        waitTimes = new float[platforms.Length];
        activeTimes = new float[platforms.Length];

        //Set the maximum order
        foreach (TimedPlatforms platform in platforms)
        {
            maxOrder = (maxOrder < platform.activationOrder) ? platform.activationOrder : maxOrder;                      
        }
        maxOrder++;

        //Get the next order of platforms to activate
        nextOrder = (currentOrder + 1) % maxOrder;

        //Loop through each platform to find the starting platforms
        for(int i = 0; i < platforms.Length; i++)
        {
            //Activate the starting platforms
            if (platforms[i].activationOrder == currentOrder)
            {
                platforms[i].platforms.SetActive(true);
                platforms[i].activeTime = timeActive;
            }
            //Deactivate the other platforms
            else
            {
                //Set the wait time of the next platform
                if (platforms[i].activationOrder == nextOrder)
                {
                    platforms[i].waitTime = nextActiveTime;
                }
                platforms[i].platforms.SetActive(false);
            }
        }

        //Increment the current order
        currentOrder++;
    }
}
