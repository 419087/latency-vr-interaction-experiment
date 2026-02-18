using UnityEngine;
using Cysharp.Threading.Tasks;

public class TaskExcuter : ITaskExcuter
{
    private readonly NetworkTaskMediator _networkTaskMediator;

    private UniTaskCompletionSource _taskSource;

    private bool _isTouchedLeftHand;
    private bool _isTouchedRightHand;

    public TaskExcuter(NetworkTaskMediator networkTaskMediator)
    {
        _networkTaskMediator = networkTaskMediator;
    }

    public async UniTask ExecuteTask(HandSide handSide)
    {
        // ここでhandSideに応じたタスクを実行するロジックを実装する
        Debug.Log($"Executing task for {handSide}");

        _isTouchedLeftHand = false;
        _isTouchedRightHand = false;

        _networkTaskMediator.ShowHandSideTextClientRpc(handSide);

        await UniTask.WaitUntil(() => IsCompleteCurrentTask(handSide));

        _networkTaskMediator.HideAllTextClientRpc();
    }

    public void TouchedHand(HandSide handSide1, HandSide handSide2)
    {
        if (handSide1 == handSide2)
        {
            if (handSide1 == HandSide.Left)
            {
                _isTouchedLeftHand = true;
            }
            else if (handSide1 == HandSide.Right)
            {
                _isTouchedRightHand = true;
            }
        }
    }

    private bool IsCompleteCurrentTask(HandSide handSide) => handSide switch
    {
        HandSide.Left => _isTouchedLeftHand,
        HandSide.Right => _isTouchedRightHand,
        HandSide.Both => _isTouchedLeftHand && _isTouchedRightHand,
        _ => false
    };
}
