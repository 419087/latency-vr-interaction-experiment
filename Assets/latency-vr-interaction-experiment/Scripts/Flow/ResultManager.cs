using UnityEngine;
using System.Collections.Generic;

public class ResultCounter
{
    private readonly string _filePath;
    private readonly PlayerData _playerData;

    private List<string> _taskHands = new List<string>();
    private List<float> _taskTimes = new List<float>();

    public ResultCounter(TaskConfigData taskConfigData, PlayerData playerData)
    {
        _filePath = taskConfigData.ResultFilePath;
        _playerData = playerData;
    }

    public void SetTaskHand(List<HandSide> handSides)
    {
        foreach (var handSide in handSides)
        {
            _taskHands.Add(handSide switch
            {
                HandSide.Left => "Left",
                HandSide.Right => "Right",
                HandSide.Both => "Both",
                _ => "Unknown"
            });
        }
    }

    public void AddTaskTime(long time)
    {
        _taskTimes.Add(time);
    }

    public void WriteResultsToCSV()
    {
        using (var writer = new System.IO.StreamWriter(_filePath + $"/id_{_playerData.GetParticipantId(0)}_{_playerData.GetParticipantId(1)}/{_playerData.LatencyCondition}.csv"))
        {
            writer.WriteLine("Task,Time(ms)");
            for (int i = 0; i < _taskHands.Count; i++)
            {
                string hand = _taskHands[i];
                float time = _taskTimes[i];
                writer.WriteLine($"{hand},{time}");
            }
        }
    }
}
