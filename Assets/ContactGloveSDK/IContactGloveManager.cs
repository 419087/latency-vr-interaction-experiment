using UnityEngine;

namespace ContactGloveSDK
{
    public interface IContactGloveManager : IControllerInput
    {
        /// <summary>
        /// Get amplitude from fingerRotationAmplitude[hand, index]
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="index"></param>
        float GetFingerRotationAmplitude(HandSides hand, FingerRotationAmplitude_e index);

        /// <summary>
        /// Set raw transform of finger
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="bone"></param>
        /// <returns></returns>
        Transform GetFingerTransform(HandSides hand, FingerRotationAmplitude_e bone);

        /// <summary>
        /// Set Vibration Style in Glove Body.
        /// </summary>
        /// <param name="hand">Left or Right</param>
        /// <param name="amplitude">0.0 ~ 1.0</param>
        /// <param name="frequency">frequency[Hz]</param>
        /// <param name="duration">duration[s]</param>
        public void SetVibration(HandSides hand, float amplitude, float frequency, float duration);

        /// <summary>
        /// Set finger haptics condition in a certain section.
        /// You must call it with enable=false once after calling with enable=true.
        /// </summary>
        /// <param name="hand">Left or Right</param>
        /// <param name="section">section</param>
        /// <param name="enable">true or false</param>
        public void SetHaptics(HandSides hand, ColliderFinger_e section, bool enable);

        public VibrationType GetVibration(HandSides hand);
        
        public Quaternion GetWristPose(HandSides hand);

    }
}