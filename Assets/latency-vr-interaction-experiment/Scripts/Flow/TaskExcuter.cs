using UnityEngine;
using Cysharp.Threading.Tasks;

public class TaskExcuter: ITaskExcuter
{
    private readonly NetworkTaskMediator _networkTaskMediator;

    private UniTaskCompletionSource _taskSource;
    private HandSide _currentHandSide;

    public TaskExcuter(NetworkTaskMediator networkTaskMediator)
    {
        _networkTaskMediator = networkTaskMediator;
    }

    public async UniTask ExecuteTask(HandSide handSide)
    {
        // ここでhandSideに応じたタスクを実行するロジックを実装する
        Debug.Log($"Executing task for {handSide}");
        _networkTaskMediator.ShowHandSideTextClientRpc(handSide);

        _currentHandSide = handSide;
        _taskSource = new UniTaskCompletionSource();

        await _taskSource.Task;
        
        _networkTaskMediator.HideAllTextClientRpc();
    }

    public void CompleteCurrentTask(HandSide handSide1, HandSide handSide2)
    {
        if (handSide1 == _currentHandSide && handSide2 == _currentHandSide)
        {
            _taskSource.TrySetResult();
        }
    }
}
