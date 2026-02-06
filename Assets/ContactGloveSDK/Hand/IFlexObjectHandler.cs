using UnityEngine;

namespace ContactGloveSDK
{
    internal interface IFlexObjectHandler
    {
        /// <summary>
        /// delegate HumanPoseHandler.GetHumanPose()
        /// </summary>
        /// <param name="humanPose"></param>
        void GetHumanPose(ref HumanPose humanPose);

        /// <summary>
        /// delegate HumanPoseHandler.SetHumanPose()
        /// </summary>
        /// <param name="humanPose"></param>
        void SetHumanPose(ref HumanPose humanPose);

        /// <summary>
        /// Set raw transform of finger
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="bone"></param>
        /// <returns></returns>
        Transform GetFingerTransform(HandSides hand, FingerRotationAmplitude_e bone);

        /// <summary>
        /// Set raw rotation of finger
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="bone"></param>
        /// <param name="rot"></param>
        void SetFingerRotation(HandSides hand, FingerRotationAmplitude_e bone, Quaternion rot);


        /// <summary>
        /// Get finger rotation amplitude
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        float GetFingerRotationAmplitude(HandSides hand, FingerRotationAmplitude_e index);


        /// <summary>
        /// Get Animator bone transform
        /// </summary>
        /// <param name="bone"></param>
        /// <returns></returns>
        Transform GetBoneTransform(HumanBodyBones bone);
    }
}