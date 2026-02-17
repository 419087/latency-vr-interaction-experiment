using UnityEngine;
using ContactGloveSDK;
using Unity.Netcode;

[System.Serializable]
public class JointData
{
    [SerializeField] private FingerRotationAmplitude_e _jointType;
    [SerializeField] private Transform _joint;

    public FingerRotationAmplitude_e JointType => _jointType;
    public Transform Joint => _joint;


    public Quaternion InitialRotation { get; private set; }
    public int JointIndex { get; private set; }     // 対応するNetworkVariableのインデックス

    // 初期回転を保存するメソッド
    public void Initialize(int jointIndex)
    {
        JointIndex = jointIndex;

        if (_joint != null)
            InitialRotation = _joint.localRotation;
    }
}
