using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

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

    public void Construct(HapticImpulsePlayer hapticImpulsePlayer)
    {
        _hapticImpulsePlayer = hapticImpulsePlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 自分がOwnerでなければ何もしない
        if (!IsOwner) return;

        // 衝突相手のNetworkObjectを取得
        var otherNetObj = other.gameObject.GetComponent<TouchInteractor>();
        if (otherNetObj == null) return;

        // 相手がOwnerでない手かどうかを確認
        if (!otherNetObj.IsOwner)
        {
            TriggerHaptic();
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