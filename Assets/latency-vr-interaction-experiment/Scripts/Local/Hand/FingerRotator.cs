using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ContactGloveSDK;
using Unity.Netcode;

public class FingerRotator : NetworkBehaviour
{
    [Header("Hand Settings")]
    [SerializeField] private HandData[] _handData;

    [Header("Rotation Settings")]
    [SerializeField] Vector3 _rotationAxis = new Vector3(0, 0, 1); // アバターによって異なる
    [SerializeField] Vector3 _thumbRotationAxis = new Vector3(0, -1, 0);      // 親指用の回転軸

    [SerializeField] float _maxRotationAngle = 80f;                // 完全に握った時の角度
    [SerializeField] float _thumbRotationAngle = 40f;      // 親指の基節の最大回転角度

    private ContactGloveManager _contactGloveManager;

    private Dictionary<HandSides, List<JointData>> _allJoints; // 「手: その手に属する関節のリスト」という形式の辞書(Startで初期化)

    private List<NetworkVariable<float>> _jointValues = new List<NetworkVariable<float>>();

    private NetworkVariable<float> _tmp = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); // デバッグ用

    public void Construct(ContactGloveManager contactGloveManager)
    {
        _contactGloveManager = contactGloveManager;
    }

    public void Awake()
    {
        Debug.Log(IsOwner + "オーナーかどうか");
        // 全ての関節データを配列にまとめる
        _allJoints = _handData.ToDictionary(
            hand => hand.HandSide,
            hand => hand.Fingers.SelectMany(finger => finger.Joints).ToList()
        );

        // 全ての関節の初期回転を保存
        // さらに、全ての関節に対応するNetworkVariableを作成して保存
        for (int i = 0; i < _allJoints.Values.SelectMany(joints => joints).Count(); i++)
        {
            JointData jointData = _allJoints.Values.SelectMany(joints => joints).ElementAt(i);
            var remoteJointValue = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
            _jointValues.Add(remoteJointValue);
            jointData.Initialize(i);
        }
    }

    // Animatorの後に実行される必要があるのでLateUpdateを使用
    private void LateUpdate()
    {
        // 全ての関節を更新
        foreach (var handEntry in _allJoints)
        {
            HandSides handSide = handEntry.Key;
            List<JointData> jointDatas = handEntry.Value;

            foreach (var jointData in jointDatas)
            {
                if (IsOwner)
                    GetJointValue(handSide, jointData); // オーナーは関節の回転を取得してNetworkVariableに保存

                UpdateFinger(handSide, jointData);
            }
        }
    }

    // 指定した関節データを基に関節を回転させるメソッド
    private void UpdateFinger(HandSides handSides, JointData jointData)
    {
        float curlValue = _jointValues[jointData.JointIndex].Value; // NetworkVariableから値を取得

        if (jointData.Joint == null)
        {
            Debug.LogWarning($"{handSides} {jointData.JointType}が設定されていません。");
            return;
        }

        Vector3 rotationAxis;
        float maxRotationAngle;

        if (jointData.JointType == FingerRotationAmplitude_e.ThumbProximal ||
            jointData.JointType == FingerRotationAmplitude_e.ThumbIntermediate ||
            jointData.JointType == FingerRotationAmplitude_e.ThumbDistal)
        {
            // 親指の関節の場合、親指用の回転軸を使用
            rotationAxis = _thumbRotationAxis;
            maxRotationAngle = _thumbRotationAngle;
        }
        else
        {
            // その他の指の関節の場合、通常の回転軸を使用
            rotationAxis = _rotationAxis;
            maxRotationAngle = _maxRotationAngle;
        }


        Quaternion targetRotation = Quaternion.Euler(rotationAxis * (curlValue * maxRotationAngle));
        jointData.Joint.localRotation = jointData.InitialRotation * targetRotation;
    }

    private void GetJointValue(HandSides handSides, JointData jointData)
    {
        float curlValue = _contactGloveManager.GetFingerRotationAmplitude(handSides, jointData.JointType);

        // デバッグ用
        if (!IsOwner)
            Debug.Log(_jointValues[jointData.JointIndex].Value);
        _jointValues[jointData.JointIndex].Value = curlValue; // NetworkVariableに値を保存
    }
}