using UnityEngine;

namespace ContactGloveSDK
{
    public interface IColliderObjectHandler
    {
        /// <summary>
        /// Create new collider object
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        GameObject CreateNewCollisionObject(Vector3 position);

        /// <summary>
        /// Get transform of bone
        /// </summary>
        /// <param name="bone"></param>
        /// <returns></returns>
        Transform GetBoneTransform(HumanBodyBones bone);
        
        /// <summary>
        /// Get finger transform (not avatar)
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="bone"></param>
        /// <returns></returns>
        Transform GetFingerTransform(HandSides hand, FingerRotationAmplitude_e bone);
        
        /// <summary>
        /// Get finger transform for hand physics slave
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="bone"></param>
        /// <returns></returns>
        Transform GetHandPhysicsFingerTransform(HandSides hand, FingerHandPhysics_e bone);
    }
}