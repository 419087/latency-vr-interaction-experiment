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
        // 1. フォルダ名とファイル名を別々に定義する（サニタイズしやすくするため）
        string folderNameRaw = $"id_{_playerData.GetParticipantId(0)}_{_playerData.GetParticipantId(1)}";
        string fileNameRaw = $"{_playerData.LatencyCondition}.csv";

        // 2. それぞれに使えない文字があれば "_" に置換
        char[] invalidChars = Path.GetInvalidFileNameChars();
        string safeFolderName = string.Join("_", folderNameRaw.Split(invalidChars));
        string safeFileName = string.Join("_", fileNameRaw.Split(invalidChars));

        // 3. 最終的なフルパスを組み立てる（指定フォルダ / IDフォルダ / ファイル名）
        string targetDirectory = Path.Combine(_filePath, safeFolderName);
        string fullPath = Path.Combine(targetDirectory, safeFileName);

        try
        {
            ExecuteSave(fullPath);
            Debug.Log($"データを保存しました: {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"指定パスへの保存に失敗({e.Message})。デスクトップへ退避します。");

            // 4. デスクトップに「ID_ファイル名」の形で保存
            string desktopPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"{safeFolderName}_{safeFileName}"
            );

            try
            {
                ExecuteSave(desktopPath);
                Debug.Log($"デスクトップに保存しました: {desktopPath}");
            }
            catch (Exception secondE)
            {
                Debug.LogError($"完全停止: {secondE.Message}");
            }
        }
    }

    // フォルダ作成と書き込みの共通ロジック
    private void ExecuteSave(string path)
    {
        string folder = Path.GetDirectoryName(path);
        // Directory.CreateDirectory は中間フォルダもすべて一気に作ってくれます
        if (!string.IsNullOrEmpty(folder))
        {
            Directory.CreateDirectory(folder);
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
