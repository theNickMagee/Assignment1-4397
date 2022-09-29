using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeCarAndCrossBridge : TaskInterface
{
    public void Execute(DeviceRegistry devices)
    {
        //This task initializes the car direction and prints out transmitter values
        //Memory Values:
        //memory[0]: flag to tell if the car has been initialized or not
        //memory[1]: flag to tell if the car is at the bridge or not
        //memory[2]: flag to tell if a boat has passed yet or not


        //variable to keep track of ego cars direction
        float angle = devices.compass[0];

        devices.steeringControl[0] = 1f;

        //If initializing the ego car...
        if (devices.memory[0] == 0)
        {

            var watch1 = new System.Diagnostics.Stopwatch();
            watch1.Start();
            Debug.Log("Initializing Car...");
            //These must be initialized to clear data from previous tests
            devices.memory[0] = 1f;
            devices.memory[1] = 0f;
            devices.memory[2] = 0f;
            devices.memory[3] = 0;
            devices.memory[8] = 0;

            //Set the direction to North
            devices.steeringControl[1] = -angle;
            watch1.Stop();
            Debug.Log($"InitializeCar's execution time: {watch1.ElapsedMilliseconds} ms");
        }
        //If NOT initializing the ego car...
        else
        {

            var watch2 = new System.Diagnostics.Stopwatch();

            //If the ego car has reached the bridge
            if (devices.memory[1] == 1 && devices.memory[8] != 1)
            {

                watch2.Start();
                devices.speedControl[0] = 1f;
                devices.brakeControl[0] = 1f;
                devices.transmitterControl[0] = 1f;
                devices.steeringControl[1] = 0f;

                //If the alarm is on, don't go yet
                if (devices.microphone[0] == 41)
                {
                    devices.brakeControl[1] = 4f;
                    devices.speedControl[1] = 0f;
                }
                //Once the alarm is off
                else
                {
                    //When the car reaches the end of the road, stop
                    if (devices.pixels[6, 0, 1] == 255 && devices.pixels[6, 14, 1] == 255)
                    {
                        devices.speedControl[1] = 0f;
                        devices.brakeControl[1] = 10f;
                        devices.memory[8] = 1f;
                    }
                    //While the car is still moving
                    else
                    {
                        //If the car is on the bridge, turn on the signal
                        if (devices.pixels[6, 7, 0] == 101)
                        {
                            devices.transmitterControl[1] = 55.6f;
                        }
                        //Once the car is no longer on the bridge, turn off the signal
                        else { devices.transmitterControl[1] = 42.5f; }
                        //Move forwards
                        devices.speedControl[1] = 4f;
                        devices.brakeControl[1] = 0f;
                    }
                }
                watch2.Stop();
                Debug.Log($"CrossBridge's execution time: {watch2.ElapsedMilliseconds} ms");
            }
        }
    }
}