using UnityEngine;
using ContactGloveSDK;

[System.Serializable]
public class JointData
{
    [SerializeField] private FingerRotationAmplitude_e _jointType;
    [SerializeField] private Transform _joint;

    public FingerRotationAmplitude_e JointType => _jointType;
    public Transform Joint => _joint;


    public Quaternion InitialRotation { get; private set; }

    // 初期回転を保存するメソッド
    public void StoreInitialRotations()
    {
        if (_joint != null)
            InitialRotation = _joint.localRotation;
    }
}
