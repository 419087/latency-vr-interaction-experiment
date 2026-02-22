using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class ResultWriter
{
    private readonly string _filePath;
    private readonly PlayerData _playerData;

    public ResultWriter(TaskConfigData taskConfigData, PlayerData playerData)
    {
        _filePath = taskConfigData.ResultFilePath;
        _playerData = playerData;
    }


    public void WriteResultsToCSV(IWritable writable, string resultType)
    {
        // 1. フォルダ名とファイル名を別々に定義する（サニタイズしやすくするため）
        string folderNameRaw = $"id_{_playerData.GetParticipantId(0)}_{_playerData.GetParticipantId(1)}";
        string fileNameRaw = $"{_playerData.LatencyCondition}_{resultType}.csv";

        // 2. それぞれに使えない文字があれば "_" に置換
        char[] invalidChars = Path.GetInvalidFileNameChars();
        string safeFolderName = string.Join("_", folderNameRaw.Split(invalidChars));
        string safeFileName = string.Join("_", fileNameRaw.Split(invalidChars));

        // 3. 最終的なフルパスを組み立てる（指定フォルダ / IDフォルダ / ファイル名）
        string targetDirectory = Path.Combine(_filePath, safeFolderName);
        string fullPath = Path.Combine(targetDirectory, safeFileName);

        try
        {
            ExecuteSave(fullPath, writable);
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
                ExecuteSave(desktopPath, writable);
                Debug.Log($"デスクトップに保存しました: {desktopPath}");
            }
            catch (Exception secondE)
            {
                Debug.LogError($"完全停止: {secondE.Message}");
            }
        }
    }

    // フォルダ作成と書き込みの共通ロジック
    private void ExecuteSave(string path, IWritable writable)
    {
        string folder = Path.GetDirectoryName(path);
        // Directory.CreateDirectory は中間フォルダもすべて一気に作ってくれます
        if (!string.IsNullOrEmpty(folder))
        {
            Directory.CreateDirectory(folder);
        }

        using (var writer = new StreamWriter(path))
        {
            writer.WriteLine(string.Join(",", writable.Columns));
            for (int i = 0; i < writable.Data.Count; i++)
            {
                writer.WriteLine(string.Join(",", writable.Data[i].Select(field => Convert.ToString(field))));
            }
        }
    }
}
