using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using System.Threading.Tasks;

public class TmpTaskExcuter: ITaskExcuter
{
    public async UniTask ExecuteTask(HandSide handSide)
    {
        // ここでhandSideに応じたタスクを実行するロジックを実装する
        Debug.Log($"Executing task for {handSide}");
        await UniTask.Delay(1000); // 例: 1秒待機するタスク
    }
}
