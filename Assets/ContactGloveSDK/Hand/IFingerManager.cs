using UnityEngine;

namespace ContactGloveSDK
{
    internal interface IFingerManager
    {
        /// <summary>
        /// Place colliders on fingers
        /// </summary>
        void MapCollider();
        
        /// <summary>
        /// Place colliders on a specific finger
        /// </summary>
        /// <param name="hand">Left or Right</param>
        void MapCollider(HandSides hand);

        /// <summary>
        /// get the name of finger from array 
        /// </summary>
        ColliderFinger_e GetFingerFromArrayIndex(int index);
        
        /// <summary>
        /// Convert collision data to the array of bytes (currently only for right hand)
        /// </summary>
        void ConvertCollisionDataToBytes(ref byte[] convertedData, bool[] collidersFlag = null);
        
    }
}