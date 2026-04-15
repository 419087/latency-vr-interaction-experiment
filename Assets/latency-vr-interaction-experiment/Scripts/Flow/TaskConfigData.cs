using UnityEngine;
public class TaskConfigData
{
    public int TaskIterations { get; }
    public int MinIntervalMilliSeconds { get; }
    public int MaxIntervalMilliSeconds { get; }
    public string ResultFilePath { get; }

    public TaskConfigData(int taskIterations, int minIntervalMilliSeconds, int maxIntervalMilliSeconds, string resultFilePath)
    {
        TaskIterations = taskIterations;
        MinIntervalMilliSeconds = minIntervalMilliSeconds;
        MaxIntervalMilliSeconds = maxIntervalMilliSeconds;
        ResultFilePath = resultFilePath;
    }
}