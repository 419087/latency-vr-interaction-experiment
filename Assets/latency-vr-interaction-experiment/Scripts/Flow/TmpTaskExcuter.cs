using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using System.Threading.Tasks;

public class TmpTaskExcuter: ITaskExcuter
{
    private readonly NetworkTaskMediator _networkTaskMediator;

    public TmpTaskExcuter(NetworkTaskMediator networkTaskMediator)
    {
        _networkTaskMediator = networkTaskMediator;
    }

    public async UniTask ExecuteTask(HandSide handSide)
    {
        // ここでhandSideに応じたタスクを実行するロジックを実装する
        Debug.Log($"Executing task for {handSide}");
        _networkTaskMediator.ShowHandSideTextClientRpc(handSide);

        await UniTask.Delay(1000); // 例: 1秒待機するタスク
        
        _networkTaskMediator.HideAllTextClientRpc();
    }
}
