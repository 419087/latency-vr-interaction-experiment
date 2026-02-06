using UnityEngine;

namespace ContactGloveSDK
{
    internal interface IFlexManager
    {
        /// <summary>
        /// initialize parameters related to bend
        /// </summary>
        /// <param name="hand"></param>
        void InitializeFingerRotation(HandSides hand);

        /// <summary>
        /// Set finger bone rotation from data given by Diving Station 
        /// </summary>
        void SetFingerRotationFromAmplitude(HandSides hand);

        /// <summary>
        /// Calculate Quaternion value of finger from rotationAmplitude
        /// </summary>
        /// <param name="rotationAmplitude"> Amplitude received from HPE</param>
        /// <returns></returns>
        Quaternion FingerRotationFromAmplitude(FingerRotationAmplitude_e bone, float rotationAmplitude, HandSides hand);

        /// <summary>
        /// Set Finger Bone Rotation from data given by Diving Station
        /// </summary>
        /// <param name="bone"> the bone to change</param>
        /// <param name="rot"> local rotation of the bone</param>
        /// <param name="fingerRotAmplitude"> Straight: 0 Bend: 1</param>
        void SetBoneRotation(FingerRotationAmplitude_e bone, float fingerRotAmplitude, HandSides hand);

        /// <summary>
        /// Set finger rotation using animator muscle method.
        /// </summary>
        /// <param name="bone"> name of the bone which will be rotated</param>
        /// <param name="rotAmplitude"> the amplitude of rotation. Straight:0.0, Bend:1.0</param>
        void SetMuscle(FingerRotationAmplitude_e bone, float rotAmplitude, HandSides hand);
    }
}