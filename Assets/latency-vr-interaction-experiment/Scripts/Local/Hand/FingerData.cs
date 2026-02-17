using UnityEngine;
using ContactGloveSDK;

[System.Serializable]
public class FingerData
{
    [SerializeField] private JointData _proximalJointData;
    [SerializeField] private JointData _intermediateJointData;
    [SerializeField] private JointData _distalJointData;

    public JointData[] Joints => new JointData[] { _proximalJointData, _intermediateJointData, _distalJointData };
}
