using ContactGloveSDK;
using UnityEngine;

public class FingerCurlManager : MonoBehaviour
{
    [SerializeField] private ContactGloveManager _gloveManager;
    [SerializeField] private FingerRotator _indexRotator;

    private void LateUpdate()
    {
        float[] curlValues = new float[3];
        curlValues[0] = _gloveManager.GetFingerRotationAmplitude(HandSides.Left, FingerRotationAmplitude_e.IndexProximal);
        curlValues[1] = _gloveManager.GetFingerRotationAmplitude(HandSides.Left, FingerRotationAmplitude_e.IndexIntermediate);
        curlValues[2] = _gloveManager.GetFingerRotationAmplitude(HandSides.Left, FingerRotationAmplitude_e.IndexDistal);

        _indexRotator.UpdateFinger(curlValues);
    }

}
