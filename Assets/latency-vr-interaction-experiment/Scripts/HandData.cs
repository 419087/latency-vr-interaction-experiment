using UnityEngine;
using ContactGloveSDK;

[System.Serializable]
public class HandData
{
    [SerializeField] private HandSides _handSide;

    [SerializeField] private FingerData _indexFinger;
    [SerializeField] private FingerData _middleFinger;
    [SerializeField] private FingerData _ringFinger;
    [SerializeField] private FingerData _littleFinger;
    [SerializeField] private FingerData _thumbFinger;

    public HandSides HandSide => _handSide;
    public FingerData[] Fingers => new FingerData[] { _indexFinger, _middleFinger, _ringFinger, _littleFinger, _thumbFinger };
}
