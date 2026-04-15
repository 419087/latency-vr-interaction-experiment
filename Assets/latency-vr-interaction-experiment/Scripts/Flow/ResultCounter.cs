using System.Collections.Generic;

public class ResultCounter
{
    private List<string> _taskHands = new List<string>();
    private List<long> _taskTimes = new List<long>();

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

    public WritableData GetWritableTaskResult()
    {
        var writable = new WritableData(new List<string> { "TaskHand", "TaskTime" }, new List<List<string>>());
        for (int i = 0; i < _taskHands.Count; i++)
        {
            writable.AddData(new List<string> { _taskHands[i], _taskTimes[i].ToString() });
        }
        
        return writable;
    }
}
