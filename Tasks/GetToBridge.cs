using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetToBridge : TaskInterface
{
    public void Execute(DeviceRegistry devices)
    {
        var watch = new System.Diagnostics.Stopwatch();

        if (devices.memory[0] == 1 && devices.memory[1] == 0)
        {
            watch.Start();
            devices.steeringControl[0] = 1f;
            devices.speedControl[0] = 1f;
            devices.brakeControl[0] = 1f;
            devices.transmitterControl[0] = 1f;

            //Get the angle to north
            float angle = devices.compass[0];

            //If at the river edge, turn west
            if(devices.pixels[6, 0, 1] == 255 && devices.pixels[6, 14, 1] == 255 && angle == 0 && devices.memory[3] == 0)
            {
                devices.steeringControl[1] = -45f;
                devices.memory[3] = 1;
            }

            //If at the bridge, turn towards it
            else if (devices.pixels[6, 0, 1] == 255 && devices.pixels[6, 14, 1] == 255 && angle <= -85 && angle >= -95)
            {
                devices.steeringControl[1] = 45f;
            }

            //Stop in front of the bridge and set the 'bridge reached' flag
            else if (((devices.pixels[6, 0, 1] == 255 && devices.pixels[6, 14, 1] == 255) && devices.memory[3] == 1))
            {
                devices.brakeControl[1] = 5f;
                devices.speedControl[1] = 0f;
                devices.steeringControl[1] = 0f;
                devices.memory[1] = 1;
                Debug.Log("Bridge Reached.");
            }

            //Move forward
            else
            {
                devices.speedControl[1] = 3f;
                devices.steeringControl[1] = 0f;
            }

            watch.Stop();
            Debug.Log($"GetToBridge's execution time: {watch.ElapsedMilliseconds} ms");
        }
    }
}