using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ContactGloveSDK
{
    /// <summary>
    /// Index of fingers
    /// </summary>
    public enum FingerNames
    {
        RightThumb = 0,
        RightIndex = 1,
        RightMiddle = 2,
        RightRing = 3,
        RightLittle = 4
    }

    /// <summary>
    /// Index of finger sections (CAUTION: different from HumanBodyBones)
    /// </summary>
    public enum ColliderFinger_e
    {
        ThumbDistal = 0,
        IndexDistal = 1,
        IndexIntermediate = 2,
        IndexProximal = 3,
        MiddleDistal = 4,
        MiddleIntermediate = 5,
        MiddleProximal = 6,
        RingDistal = 7,
        RingIntermediate = 8,
        RingProximal = 9,
        LittleDistal = 10
    }

    internal static class FingerSectionNamesExt
    {
        internal static HumanBodyBones ToHumanBodyBone(this ColliderFinger_e colliderFingerE, HandSides hand)
        {
            if (hand == HandSides.Right)
            {
                switch (colliderFingerE)
                {
                    case ColliderFinger_e.ThumbDistal:
                        return HumanBodyBones.RightThumbDistal;
                    case ColliderFinger_e.IndexDistal:
                        return HumanBodyBones.RightIndexDistal;
                    case ColliderFinger_e.IndexIntermediate:
                        return HumanBodyBones.RightIndexIntermediate;
                    case ColliderFinger_e.IndexProximal:
                        return HumanBodyBones.RightIndexProximal;
                    case ColliderFinger_e.MiddleDistal:
                        return HumanBodyBones.RightMiddleDistal;
                    case ColliderFinger_e.MiddleIntermediate:
                        return HumanBodyBones.RightMiddleIntermediate;
                    case ColliderFinger_e.MiddleProximal:
                        return HumanBodyBones.RightMiddleProximal;
                    case ColliderFinger_e.RingDistal:
                        return HumanBodyBones.RightRingDistal;
                    case ColliderFinger_e.RingIntermediate:
                        return HumanBodyBones.RightRingIntermediate;
                    case ColliderFinger_e.RingProximal:
                        return HumanBodyBones.RightRingProximal;
                    case ColliderFinger_e.LittleDistal:
                        return HumanBodyBones.RightLittleDistal;
                }
            }
            else
            {
                switch (colliderFingerE)
                {
                    case ColliderFinger_e.ThumbDistal:
                        return HumanBodyBones.LeftThumbDistal;
                    case ColliderFinger_e.IndexDistal:
                        return HumanBodyBones.LeftIndexDistal;
                    case ColliderFinger_e.IndexIntermediate:
                        return HumanBodyBones.LeftIndexIntermediate;
                    case ColliderFinger_e.IndexProximal:
                        return HumanBodyBones.LeftIndexProximal;
                    case ColliderFinger_e.MiddleDistal:
                        return HumanBodyBones.LeftMiddleDistal;
                    case ColliderFinger_e.MiddleIntermediate:
                        return HumanBodyBones.LeftMiddleIntermediate;
                    case ColliderFinger_e.MiddleProximal:
                        return HumanBodyBones.LeftMiddleProximal;
                    case ColliderFinger_e.RingDistal:
                        return HumanBodyBones.LeftRingDistal;
                    case ColliderFinger_e.RingIntermediate:
                        return HumanBodyBones.LeftRingIntermediate;
                    case ColliderFinger_e.RingProximal:
                        return HumanBodyBones.LeftRingProximal;
                    case ColliderFinger_e.LittleDistal:
                        return HumanBodyBones.LeftLittleDistal;
                }
            }

            return HumanBodyBones.RightThumbDistal;
        }

        internal static FingerRotationAmplitude_e ToFingerRotationAmplitudeE(this ColliderFinger_e colliderFingerE)
        {
            switch (colliderFingerE)
            {
                case ColliderFinger_e.ThumbDistal:
                    return FingerRotationAmplitude_e.ThumbDistal;
                case ColliderFinger_e.IndexDistal:
                    return FingerRotationAmplitude_e.IndexDistal;
                case ColliderFinger_e.IndexIntermediate:
                    return FingerRotationAmplitude_e.IndexIntermediate;
                case ColliderFinger_e.IndexProximal:
                    return FingerRotationAmplitude_e.IndexProximal;
                case ColliderFinger_e.MiddleDistal:
                    return FingerRotationAmplitude_e.MiddleDistal;
                case ColliderFinger_e.MiddleIntermediate:
                    return FingerRotationAmplitude_e.MiddleIntermediate;
                case ColliderFinger_e.MiddleProximal:
                    return FingerRotationAmplitude_e.MiddleProximal;
                case ColliderFinger_e.RingDistal:
                    return FingerRotationAmplitude_e.RingDistal;
                case ColliderFinger_e.RingIntermediate:
                    return FingerRotationAmplitude_e.RingIntermediate;
                case ColliderFinger_e.RingProximal:
                    return FingerRotationAmplitude_e.RingProximal;
                case ColliderFinger_e.LittleDistal:
                    return FingerRotationAmplitude_e.LittleDistal;
                default:
                    Debug.Log("Unknown colliderFingerE: " + colliderFingerE);
                    return FingerRotationAmplitude_e.ThumbDistal;
            }
        }
        internal static FingerHandPhysics_e ToFingerHandPhysics(this ColliderFinger_e bone)
        {
            FingerHandPhysics_e fingerHandPhysicsE = FingerHandPhysics_e.Thumb;
            switch (bone)
            {
                case ColliderFinger_e.IndexDistal:
                    fingerHandPhysicsE = FingerHandPhysics_e.Index;
                    break;
                case ColliderFinger_e.MiddleDistal:
                    fingerHandPhysicsE = FingerHandPhysics_e.Middle;
                    break;
                case ColliderFinger_e.RingDistal:
                    fingerHandPhysicsE = FingerHandPhysics_e.Ring;
                    break;
                case ColliderFinger_e.ThumbDistal:
                    fingerHandPhysicsE = FingerHandPhysics_e.Thumb;
                    break;
                default:
                    break;
            }

            return fingerHandPhysicsE;
        }

        internal static int ColliderNum(this ColliderFinger_e colliderFingerE)
        {
            switch (colliderFingerE)
            {
                case ColliderFinger_e.IndexDistal:
                case ColliderFinger_e.MiddleDistal:
                case ColliderFinger_e.RingDistal:
                case ColliderFinger_e.ThumbDistal:
                    return 4;
                default:
                    return 0;
            }
        }

        internal static List<int> ColliderIndices(this ColliderFinger_e colliderFingerE)
        {
            switch (colliderFingerE)
            {
                case ColliderFinger_e.ThumbDistal:
                    return new List<int> { 0, 1, 2, 3 };
                case ColliderFinger_e.IndexDistal:
                    return new List<int>{4, 5, 6, 7};
                case ColliderFinger_e.MiddleDistal:
                    return new List<int>{8, 9, 10, 11};
                case ColliderFinger_e.RingDistal:
                    return new List<int>{12, 13, 14, 15};
                default:
                    Debug.Log("Unknown colliderFingerE: " + colliderFingerE);
                    break;
            }

            return new List<int> { };
        }
    }

    /// <summary>
    /// Index of the colliders on each finger section
    /// </summary>
    public enum HapticModules
    {
        topLeft = 0,
        intermediateLeft = 1,
        bottomLeft = 2,
        topRight = 3,
        intermediateRight = 4,
        bottomRight = 5
    }

    /*
    /// <summary>
    /// Pattern
    /// </summary>
    public static class HapticPattern
    {
        /// <summary>
        /// Calculate the index of the first section in the designated finger
        /// </summary>
        /// <param name="finger">Finger's name defined in enum FingerNames</param>
        /// <returns></returns>
        private static int GetIndexOfFirstSection(this FingerManager fingerManager, FingerNames finger)
        {
            int startIndex = 0;
            for (int i = 0; i < (int)finger; ++i)
            {
                startIndex += fingerManager._colliders.numberOfSectionsPerFinger[i];
            }

            return startIndex;
        }

        /// <summary>
        /// Turn on the designated collider, wait for seconds, and turn it off.<br></br>
        /// intermediateLeft and intermediateRight are available only when a finger section has 6 colliders.<br></br>
        /// If the arguments are invalid, return immediately.
        /// </summary>
        /// <param name="section">Finger section's name defined in enum FingerSectionNames</param>
        /// <param name="collider">Collider's name defined in enum ColliderNames</param>
        /// <param name="duration">(Optional) Duration in second. The default value is 1.0 sec.</param>
        /// <returns></returns>
        private static IEnumerator RunCollision(this FingerManager fingerManager, FingerSectionNames section,
            ColliderNames collider,
            float duration = 1.0f)
        {
            fingerManager._colliders.SetColliderOn(section, collider);

            // Wait
            yield return new WaitForSeconds(duration);

            fingerManager._colliders.SetColliderOff(section, collider);
        }

        /// <summary>
        /// Turn on colliders in row in the designated finger from the top to bottom.
        /// </summary>
        /// <param name="finger">Finger's name defined in enum FingerNames</param>
        /// <param name="interval">(Optional) Interval between a row and the next row. The default value is 1.0 sec.</param>
        /// <param name="duration">(Optional) Duration in second. The default value is 1.0 sec.</param>
        /// <returns></returns>
        public static IEnumerator RunCollisionForRows(this FingerManager fingerManager, FingerNames finger,
            float interval = 1.0f, float duration = 1.0f)
        {
            // Call RunCollision() for each collider row in the designated finger
            int startIndex = fingerManager.GetIndexOfFirstSection(finger);
            for (int i = 0; i < fingerManager._colliders.numberOfSectionsPerFinger[(int)finger]; ++i)
            {
                bool isIntermediateExist = fingerManager._colliders.numbersOfCollidersPerSection[startIndex + i] == 4;
                for (int j = 0; j < Colliders.maxNumberOfCollidersPerSection / 2; ++j)
                {
                    if (!isIntermediateExist && j == 1)
                    {
                        continue;
                    }

                    fingerManager.StartCoroutine(fingerManager.RunCollision((FingerSectionNames)startIndex + i,
                        (ColliderNames)j,
                        duration));
                    fingerManager.StartCoroutine(fingerManager.RunCollision((FingerSectionNames)startIndex + i,
                        (ColliderNames)j + 3,
                        duration));
                    yield return new WaitForSeconds(interval);
                }
            }

            yield break;
        }

        /// <summary>
        /// Turn on all colliders in the designated section, wait for seconds, and turn them off.
        /// </summary>
        /// <param name="section">Finger section's name defined in enum FingerSectionNames</param>
        /// <param name="duration">(Optional) Duration in second. The default value is 1.0 sec.</param>
        /// <returns></returns>
        private static IEnumerator RunCollisionInSection(this FingerManager fingerManager, FingerSectionNames section,
            float duration = 1.0f)
        {
            // Call RunCollision() for each collider in the designated section
            bool isIntermediateExist = fingerManager._colliders.numbersOfCollidersPerSection[(int)section] == 4;
            for (int i = 0; i < Colliders.maxNumberOfCollidersPerSection; ++i)
            {
                // If the finger section contains 4 colliders, calculate the diff of index
                if (!isIntermediateExist &&
                    (i == (int)ColliderNames.intermediateLeft || i == (int)ColliderNames.intermediateRight))
                {
                    continue;
                }

                fingerManager.StartCoroutine(fingerManager.RunCollision(section, (ColliderNames)i, duration));
            }

            yield break;
        }

        /// <summary>
        /// Turn on all colliders in the designated finger, wait for seconds, and turn them off.
        /// </summary>
        /// <param name="finger">Finger's name defined in enum FingerNames</param>
        /// <param name="duration">(Optional) Duration in second. The default value is 1.0 sec.</param>
        /// <returns></returns>
        private static IEnumerator RunCollisionInFinger(this FingerManager fingerManager, FingerNames finger,
            float duration = 1.0f)
        {
            // Call RunCollisionInSection() for each finger section in the designated finger
            int startIndex = fingerManager.GetIndexOfFirstSection(finger);
            for (int i = 0; i < fingerManager._colliders.numberOfSectionsPerFinger[(int)finger]; ++i)
            {
                fingerManager.StartCoroutine(
                    fingerManager.RunCollisionInSection((FingerSectionNames)(startIndex + i), duration));
            }

            yield break;
        }
    }
    */
}