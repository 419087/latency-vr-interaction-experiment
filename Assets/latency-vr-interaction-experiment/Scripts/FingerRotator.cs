using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ContactGloveSDK;

public class FingerRotator : MonoBehaviour
{
    [SerializeField] private ContactGloveManager _contactGloveManager;

    [Header("Hand Settings")]
    [SerializeField] private HandData[] _handData;

    [Header("Rotation Settings")]
    [SerializeField] Vector3 rotationAxis = new Vector3(0, 0, 1); // アバターによって異なる
    [SerializeField] float maxRotationAngle = 80f;                // 完全に握った時の角度

    private Dictionary<HandSides, List<JointData>> _allJoints; // 「手: その手に属する関節のリスト」という形式の辞書(Startで初期化)


    void Start()
    {
        // 全ての関節データを配列にまとめる
        _allJoints = _handData.ToDictionary(
            hand => hand.HandSide,
            hand => hand.Fingers.SelectMany(finger => finger.Joints).ToList()
        );

        // 全ての関節の初期回転を保存
        foreach (var jointData in _allJoints.Values.SelectMany(joints => joints))
        {
            jointData.StoreInitialRotations();
        }
    }

    // Animatorの後に実行される必要があるのでLateUpdateを使用
    public void LateUpdate()
    {
        // 全ての関節を更新
        foreach (var handEntry in _allJoints)
        {
            HandSides handSide = handEntry.Key;
            List<JointData> jointDatas = handEntry.Value;

            foreach (var jointData in jointDatas)
            {
                UpdateFinger(handSide, jointData);
            }
        }
    }

    // 指定した関節データを基に関節を回転させるメソッド
    public void UpdateFinger(HandSides handSides, JointData jointData)
    {
        float curlValue = _contactGloveManager.GetFingerRotationAmplitude(handSides, jointData.JointType);

        Quaternion targetRotation = Quaternion.Euler(rotationAxis * (curlValue * maxRotationAngle));
        jointData.Joint.localRotation = jointData.InitialRotation * targetRotation;
    }
}