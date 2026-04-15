using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using System.Threading.Tasks;

public class TmpTaskExcuter: ITaskExcuter
{
    private readonly NetworkTaskMediator _networkTaskMediator;
    private readonly ResultCounter _resultManager;

    public TmpTaskExcuter(NetworkTaskMediator networkTaskMediator, ResultCounter resultManager)
    {
        _networkTaskMediator = networkTaskMediator;
        _resultManager = resultManager;
    }

    public async UniTask ExecuteTask(HandSide handSide)
    {
        // ここでhandSideに応じたタスクを実行するロジックを実装する
        Debug.Log($"Executing task for {handSide}");
        _networkTaskMediator.ShowHandSideTextClientRpc(handSide);

        await UniTask.Delay(1000); // 例: 1秒待機するタスク
        _resultManager.AddTaskTime(1000);
        
        _networkTaskMediator.HideAllTextClientRpc();
    }

    public void TouchedHand(HandSide handSide1, HandSide handSide2)
    {
        // このクラスではタスクの完了を管理しないため、空実装とする
    }
}
