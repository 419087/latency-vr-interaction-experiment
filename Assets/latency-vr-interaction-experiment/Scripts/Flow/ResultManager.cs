using UnityEngine;
using System.Collections.Generic;

public class ResultCounter
{
    private List<float> _taskTimes = new List<float>();

    public void AddTaskTime(long time)
    {
        _taskTimes.Add(time);
    }
}
