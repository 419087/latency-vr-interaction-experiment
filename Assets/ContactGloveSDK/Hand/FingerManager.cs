using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
 
namespace ContactGloveSDK
{
   /// <summary>
   /// Finger class (inherits Hand class)
   /// This class does
   /// 1. Inherits Hand class
   /// 2. Map Colliders on fingers
   /// </summary>
    internal class FingerManager : IFingerManager, IHand
    {
        private readonly FingerTipCollisionDetection[] collisionArray =
            new FingerTipCollisionDetection[DataLength.CollisionNum];
    
        private readonly IColliderObjectHandler colliderObjectHandler;
    
        private readonly bool useAnimator;
        private readonly bool useHandPhysics;
        
        private Vector3[] sixOffsetL;
        private Vector3[] sixOffsetR;

        private Vector3[] fourOffsetL;
        private Vector3[] fourOffsetR;
    
        internal FingerManager(IColliderObjectHandler colliderObjectHandler, bool useAnimator, bool useHandPhysics)
        {
            // colliders = new Colliders();
            this.colliderObjectHandler = colliderObjectHandler;
            this.useAnimator = useAnimator;
            this.useHandPhysics = useHandPhysics;
            
            this.SetOffset();
        }

        private void SetOffset()
        {
            sixOffsetL = new Vector3[]
                {
                    new Vector3(1, 0, 1), new Vector3(0, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1),
                    new Vector3(0, 0, -1), new Vector3(-1, 0, -1)
                };
            sixOffsetR = new Vector3[]
                {
                    new Vector3(1, 0, 1), new Vector3(0, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1),
                    new Vector3(0, 0, -1), new Vector3(-1, 0, -1)
                };
            
            fourOffsetL = new Vector3[]
                { new Vector3(1, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1) };
            fourOffsetR = new Vector3[]
                    { new Vector3(1, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1) };
            
            foreach (HandSides hand in Enum.GetValues(typeof(HandSides)))
            {
                const float spaceBetweenCollision = .006f;
                const float spaceBetweenCollisionZ = .8f;
                Vector3 xyHandOffset = new Vector3(2 * spaceBetweenCollision * (hand == HandSides.Left ? -1 : 1),
                    spaceBetweenCollision * (hand == HandSides.Left ? 1 : -1), 0);
                // Adjust positions of offsets
                
                var sixOffset = hand == HandSides.Left ? sixOffsetL : sixOffsetR;
                var fourOffset = hand == HandSides.Left ? fourOffsetL : fourOffsetR;
                
                for (int i = 0; i < sixOffset.Length; ++i)
                {
                    sixOffset[i] *= spaceBetweenCollision;
                    sixOffset[i].z *= spaceBetweenCollisionZ;
                    sixOffset[i] += xyHandOffset;
                }

                for (int i = 0; i < fourOffset.Length; ++i)
                {
                    fourOffset[i] *= spaceBetweenCollision;
                    fourOffset[i].z *= spaceBetweenCollisionZ;
                    fourOffset[i] += xyHandOffset;
                }
            }
        }

        public void MapCollider(HandSides hand)
        {
            int colliderNumberCnt = hand == HandSides.Left ? 0 : DataLength.CollisionNum / 2;
            
            foreach (ColliderFinger_e fingerSection in Enum.GetValues(typeof(ColliderFinger_e)))
            {
                Transform fingerTransform;
                if (useAnimator)
                {
                    fingerTransform =
                        colliderObjectHandler.GetBoneTransform(fingerSection.ToHumanBodyBone(hand));
                }
                else if (useHandPhysics)
                {
                    fingerTransform =
                        colliderObjectHandler.GetHandPhysicsFingerTransform(hand, fingerSection.ToFingerHandPhysics());
                }
                else
                {
                    fingerTransform = colliderObjectHandler.GetFingerTransform(hand,
                        fingerSection.ToFingerRotationAmplitudeE());
                }

                // Detect null
                if (fingerTransform == null)
                {
                    Debug.Log("MapCollider(): " + hand + " " + (int)fingerSection + "th finger is null");
                    continue;
                }

                // Set the offset based on the number of colliders the finger has
                Vector3[] colliderPositionOffsets;

                if (fingerSection.ColliderNum() == 6)
                    colliderPositionOffsets = hand == HandSides.Left ? sixOffsetL : sixOffsetR;
                else if (fingerSection.ColliderNum() == 4)
                    colliderPositionOffsets = hand == HandSides.Left ? fourOffsetL : fourOffsetR;
                else
                    continue;

                // Set colliders
                foreach (var offset in colliderPositionOffsets)
                {
                    // Create the collision
                    GameObject newCollision =
                        colliderObjectHandler.CreateNewCollisionObject(fingerTransform.position + offset);
                    newCollision.transform.parent = fingerTransform;
                    newCollision.name = "FingerCollider" + colliderNumberCnt;
                    
                    // Store the collision in the array
                    collisionArray[colliderNumberCnt] = newCollision.GetComponent<FingerTipCollisionDetection>();
                    collisionArray[colliderNumberCnt].hand = hand;
                    ++colliderNumberCnt;
                }
            }
        }

        public void MapCollider()
        {
            // Set colliders
            MapCollider(HandSides.Left);
            MapCollider(HandSides.Right);
        }
    
        /// <summary>
        /// Get the finger assigned in each collider
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ColliderFinger_e GetFingerFromArrayIndex(int index)
        {
            int colliderNumberCnt = 0;
    
            foreach (HandSides hand in Enum.GetValues(typeof(HandSides)))
            {
                foreach (ColliderFinger_e fingerSection in Enum.GetValues(typeof(ColliderFinger_e)))
                {
                    // get collider number for each collider parts
                    int colliderQuantity = fingerSection.ColliderNum();
    
                    // Set colliders
                    for (int j = 0; j < colliderQuantity; ++j)
                    {
                        if (colliderNumberCnt == index)
                        {
                            return fingerSection;
                        }
    
                        ++colliderNumberCnt;
                    }
                }
            }
    
            Debug.Log("GetFingerFromArrayIndex(): index is out of range " + index);
            return ColliderFinger_e.ThumbDistal;
        }
    
        public void ConvertCollisionDataToBytes(ref byte[] convertedData, bool[] collidersFlag = null)
        {
            // putting into another array to modify it
            // doesn't have to be like this if you don't need to turn on all the colliders in the same finger
            bool[] temporaryCollisionArray = new bool[collisionArray.Length];
            
            if (collidersFlag != null)
            {
                Assert.AreEqual(collidersFlag.Length, DataLength.CollisionNum);
                for (int i = 0; i < collisionArray.Length; i++)
                {
                    temporaryCollisionArray[i] = collidersFlag[i];
                }
            }
            else
            {
                for (int i = 0; i < collisionArray.Length; ++i)
                {
                    HandSides hand = i < collisionArray.Length / 2 ? HandSides.Left : HandSides.Right;
                    int rightThumbIndex = collisionArray.Length / 2;
                    int leftThumbIndex = 0;
                    
                    // The beginning index for each hand. The first half shows the collision data for left hand,
                    // and the second half shows the collision data for right hand.
                    
                    int handBeginningIndex = hand == HandSides.Right ? rightThumbIndex : leftThumbIndex;

                    if (collisionArray[i] != null)
                    {
                        temporaryCollisionArray[i] |= collisionArray[i].isColliding;
                    }
                    
                    // set the flag to 1 for all the colliders in the same finger
                    if (temporaryCollisionArray[i])
                    {
                        ColliderFinger_e currentFinger = GetFingerFromArrayIndex(i);
    
                        // Set the flag for all the colliders in the same finger in the same hand
                        for (int j = handBeginningIndex; j < handBeginningIndex + collisionArray.Length/2; ++j)
                        {
                            if (currentFinger == GetFingerFromArrayIndex(j))
                            {
                                temporaryCollisionArray[j] = true;
                            }
                        }
                    }
                }
            }
            
            convertedData = HapticsManager.ConvertBitsToBytes(temporaryCollisionArray);
            // GetCollisionsWithRay(ref convertedData);
        }
        
        public void GetCollisionsWithRay(ref byte[] convertedData)
        {
            bool[] temporaryCollisionArray = new bool[collisionArray.Length];
            int colliderNumberCnt = 0;
            
            foreach (HandSides hand in Enum.GetValues(typeof(HandSides)))
            {
                foreach (ColliderFinger_e fingerSection in Enum.GetValues(typeof(ColliderFinger_e)))
                {
                    Transform fingerTransform;
                    if (useAnimator)
                    {
                        fingerTransform =
                            colliderObjectHandler.GetBoneTransform(fingerSection.ToHumanBodyBone(hand));
                    }
                    else if (useHandPhysics)
                    {
                        fingerTransform =
                            colliderObjectHandler.GetHandPhysicsFingerTransform(hand,
                                fingerSection.ToFingerHandPhysics());
                    }
                    else
                    {
                        fingerTransform = colliderObjectHandler.GetFingerTransform(hand,
                            fingerSection.ToFingerRotationAmplitudeE());
                    }

                    // Detect null
                    if (fingerTransform == null)
                    {
                        Debug.Log("MapCollider(): " + hand + " " + (int)fingerSection + "th finger is null");
                        continue;
                    }

                    // Set the offset based on the number of colliders the finger has
                    Vector3[] colliderPositionOffsets;

                    if (fingerSection.ColliderNum() == 6)
                        colliderPositionOffsets = hand == HandSides.Left ? sixOffsetL : sixOffsetR;
                    else if (fingerSection.ColliderNum() == 4)
                        colliderPositionOffsets = hand == HandSides.Left ? fourOffsetL : fourOffsetR;
                    else
                        continue;

                    // Set colliders
                    foreach (var offset in colliderPositionOffsets)
                    {
                        Debug.Log("offset:" + offset);
                        Vector3 origin = fingerTransform.TransformPoint(offset);
                        Vector3 dir = fingerTransform.TransformPoint(new Vector3(0, 0, offset.z));
                        Ray ray = new Ray(origin, dir);
                        RaycastHit[] hit = Physics.SphereCastAll(ray, 0.01f, 0.01f);
                        // Debug.DrawRay(origin, dir, Color.red, 1.0f);
                        bool isHit = false;
                        foreach (var obj in hit)
                        {
                            if (obj.transform.tag != "FingerCollider")
                            {
                                isHit = true;
                            }
                        }

                        temporaryCollisionArray[colliderNumberCnt++] = isHit;
                    }
                }
            }
            
            convertedData = HapticsManager.ConvertBitsToBytes(temporaryCollisionArray);
        }

        public void SetWristPose(HandSides side, Vector3 newPoseVector, Quaternion newPoseRotation)
        {
            Transform wristTransform;
            if (side == HandSides.Left)
            {
                wristTransform = colliderObjectHandler.GetBoneTransform(HumanBodyBones.LeftHand);
            }
            else if (side == HandSides.Right)
            {
                wristTransform = colliderObjectHandler.GetBoneTransform(HumanBodyBones.RightHand);
            }
            else
            {
                Debug.Log("SetWristPose(): Invalid side");
                return;
            }
    
            wristTransform.position = newPoseVector;
            wristTransform.rotation = newPoseRotation;
        }
    }
}

