using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using System.Diagnostics;

public class TaskExcuter : ITaskExcuter
{
    private readonly NetworkTaskMediator _networkTaskMediator;
    private readonly NetworkManager _networkManager;

    private readonly PlayerData _playerData;
    private readonly ResultCounter _resultManager;
    private bool _isTouchedLeftHand;
    private bool _isTouchedRightHand;

    public TaskExcuter(NetworkTaskMediator networkTaskMediator, NetworkManager networkManager, PlayerData playerData, ResultCounter resultManager)
    {
        _networkTaskMediator = networkTaskMediator;
        _networkManager = networkManager;
        _playerData = playerData;
        _resultManager = resultManager;
    }

    public async UniTask ExecuteTask(HandSide handSide)
    {
        // ここでhandSideに応じたタスクを実行するロジックを実装する
        UnityEngine.Debug.Log($"Executing task for {handSide}");

        _isTouchedLeftHand = false;
        _isTouchedRightHand = false;

        SendClientHandSideText(handSide, 0);
        SendClientHandSideText(handSide.Opposite(), 1);

        // 時間を計測する
        Stopwatch sw = Stopwatch.StartNew();

        await UniTask.WaitUntil(() => IsCompleteCurrentTask(handSide));

        sw.Stop();
        _resultManager.AddTaskTime(sw.ElapsedMilliseconds);
        UnityEngine.Debug.Log($"Task execution time: {sw.ElapsedMilliseconds} ms");

        _networkTaskMediator.HideAllTextClientRpc();
    }

    public void TouchedHand(HandSide handSide1, HandSide handSide2)
    {
        if (handSide1 == handSide2.Opposite())
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

    private void SendClientHandSideText(HandSide handSide, int playerId)
    {
        ulong? clientId = _playerData.GetClientId(playerId);
        if (!clientId.HasValue)
        {
            UnityEngine.Debug.LogError($"PlayerId {playerId} に対応するClientIdが見つかりません。");
            return;
        }

        ClientRpcParams rpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId.Value }
            }
        };

        _networkTaskMediator.ShowHandSideTextClientRpc(handSide, rpcParams);
    }
}
