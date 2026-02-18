using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using System.Threading.Tasks;

public class TouchInteractor : NetworkBehaviour
{
    [SerializeField] private HandSide _handSide;
    public HandSide HandSide => _handSide;

    [Header("Haptic Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _hapticAmplitude = 1f;
    [SerializeField] private float _hapticDuration = 0.1f;

    // コントローラーの振動システムをアサイン
    private HapticImpulsePlayer _hapticImpulsePlayer;
    // サーバーの場合はTaskExcuterをアサイン
    private ITaskExcuter _taskExcuter;

    public void ConstructServer(HapticImpulsePlayer hapticImpulsePlayer, ITaskExcuter taskExcuter)
    {
        Debug.Log(taskExcuter == null ? "null" : "not null");
        _hapticImpulsePlayer = hapticImpulsePlayer;
        _taskExcuter = taskExcuter;
    }

    public void ConstructClient(HapticImpulsePlayer hapticImpulsePlayer)
    {
        _hapticImpulsePlayer = hapticImpulsePlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 自分がOwnerかサーバー側でなければ何もしない
        if (!IsOwner && !IsServer) return;

        // 衝突相手のNetworkObjectを取得
        var otherNetObj = other.gameObject.GetComponent<TouchInteractor>();
        if (otherNetObj == null) return;

        // 相手が異なるオーナーの手かどうかを確認
        if (this.OwnerClientId != otherNetObj.OwnerClientId)
        {
            if (IsServer)
            {
                _taskExcuter.CompleteCurrentTask(_handSide, otherNetObj.HandSide);
            }
            else if (IsOwner)
            {
                TriggerHaptic();
            }
        }
    }

    private void TriggerHaptic()
    {
        if (_hapticImpulsePlayer != null)
        {
            Debug.Log("振動");
            _hapticImpulsePlayer.SendHapticImpulse(_hapticAmplitude, _hapticDuration);
        }
    }
}