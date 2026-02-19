using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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
        // 1. ファイル名を作成（この時点ではまだ安全ではない可能性がある）
        string rawFileName = $"id_{_playerData.GetParticipantId(0)}_{_playerData.GetParticipantId(1)}_{_playerData.LatencyCondition}.csv";

        // 2. ファイル名に使えない文字が含まれていたら "_" に置き換える（サニライズ）
        string safeFileName = string.Join("_", rawFileName.Split(Path.GetInvalidFileNameChars()));

        // 3. 本来の保存先パスを生成
        string savePath = Path.Combine(_filePath, safeFileName);

        try
        {
            // 指定された場所への保存を試みる
            ExecuteSave(savePath);
            Debug.Log($"結果を保存しました: {savePath}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"指定パスへの保存に失敗しました({e.Message})。デスクトップへの保存に切り替えます。");

            // 4. 失敗した場合、デスクトップ直下に保存先を変更
            string desktopPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "BACKUP_" + safeFileName
            );

            try
            {
                ExecuteSave(desktopPath);
                Debug.Log($"デスクトップに結果を保存しました: {desktopPath}");
            }
            catch (Exception secondE)
            {
                Debug.LogError($"デスクトップへの保存も失敗しました。権限等を確認してください: {secondE.Message}");
            }
        }
    }

    // 実際の書き出し処理を分離（再利用と可読性のため）
    private void ExecuteSave(string path)
    {
        string folderPath = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        using (var writer = new StreamWriter(path))
        {
            writer.WriteLine("Task,Time(ms)");
            for (int i = 0; i < _taskHands.Count; i++)
            {
                writer.WriteLine($"{_taskHands[i]},{_taskTimes[i]}");
            }
        }
    }
}
