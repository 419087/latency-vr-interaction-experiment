using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ContactGloveSDK
{
    /// <summary>
    /// Definition of sides of the hand
    /// </summary>
    public enum HandSides
    {
        Left = 0,
        Right = 1
    }

    interface IHand
    {
        /// <summary>
        /// Set wrist transform
        /// </summary>
        /// <param name="side">Left or Right</param>
        /// <param name="newPoseVector">Vector of wrist</param>
        /// <param name="newPoseRotation">Quaternion of wrist</param>
        void SetWristPose(HandSides side, Vector3 newPoseVector, Quaternion newPoseRotation);
    }
}