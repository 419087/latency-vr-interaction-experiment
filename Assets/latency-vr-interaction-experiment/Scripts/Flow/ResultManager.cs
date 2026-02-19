using UnityEngine;
using System.Collections.Generic;
using System.IO;

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
        // ファイル名に参加者IDとレイテンシ条件を含める
        string savePath = $"{_filePath}/id_{_playerData.GetParticipantId(0)}_{_playerData.GetParticipantId(1)}_{_playerData.LatencyCondition}.csv";

        string folderPath = Path.GetDirectoryName(savePath);
        
        // フォルダが存在しない場合は作成する
        if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        using (var writer = new System.IO.StreamWriter(savePath))  
        {
            writer.WriteLine("Task,Time(ms)");
            for (int i = 0; i < _taskHands.Count; i++)
            {
                string hand = _taskHands[i];
                float time = _taskTimes[i];
                writer.WriteLine($"{hand},{time}");
            }
        }

        Debug.Log($"結果を保存しました: {savePath}");
    }
}
