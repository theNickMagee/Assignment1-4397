using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    protected TaskInterface[] tasks = new TaskInterface[] {
        new InitializeCarAndCrossBridge(),
        new GetToBridge(),
        //new Debug_Task() //This task was part of the assignment. Leaving it in to show that it has been tested.
    };

    public TaskInterface[] GetTasks()
    {
        return this.tasks;
    }
}
