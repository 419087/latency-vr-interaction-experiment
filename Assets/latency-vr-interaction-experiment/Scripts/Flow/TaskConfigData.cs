using UnityEngine;
public class TaskConfigData
{
    public int TaskIterations { get; }
    public int MinIntervalMilliSeconds { get; }
    public int MaxIntervalMilliSeconds { get; }

    public TaskConfigData(int taskIterations, int minIntervalMilliSeconds, int maxIntervalMilliSeconds)
    {
        TaskIterations = taskIterations;
        MinIntervalMilliSeconds = minIntervalMilliSeconds;
        MaxIntervalMilliSeconds = maxIntervalMilliSeconds;
    }
}