using System.Collections.Generic;
using UnityEngine;

namespace ContactGloveSDK
{
    /// <summary>
    /// Enum of finger rotation itself. Index: order of finger rotation amplitude.
    /// </summary>
    public enum FingerRotationAmplitude_e : int
    {
        LittleProximal = 0,
        LittleIntermediate = 1,
        LittleDistal = 2,
        RingProximal = 3,
        RingIntermediate = 4,
        RingDistal = 5,
        MiddleProximal = 6,
        MiddleIntermediate = 7,
        MiddleDistal = 8,
        IndexProximal = 9,
        IndexIntermediate = 10,
        IndexDistal = 11,
        ThumbProximal = 12,
        ThumbIntermediate = 13,
        ThumbDistal = 14,
        ThumbAbduction = 15,
        LittleSplay = 16,
        RingSplay = 17,
        MiddleSplay = 18,
        IndexSplay = 19,
        ThumbSplay = 20
    }

    public class FlexData
    {
        public readonly HandSides Hand;
        public readonly bool Valid;
        private readonly IDictionary<FingerRotationAmplitude_e, float> _amplitudes;

        public FlexData(HandSides hand, NamedPipe.HandPose pose)
        {
            this.Hand = hand;
            this.Valid = true;
            _amplitudes = new Dictionary<FingerRotationAmplitude_e, float>
            {
                {FingerRotationAmplitude_e.LittleProximal, pose.little.proximal},
                {FingerRotationAmplitude_e.LittleIntermediate, pose.little.intermediate},
                {FingerRotationAmplitude_e.LittleDistal, pose.little.distal},
                {FingerRotationAmplitude_e.RingProximal, pose.ring.proximal},
                {FingerRotationAmplitude_e.RingIntermediate, pose.ring.intermediate},
                {FingerRotationAmplitude_e.RingDistal, pose.ring.distal},
                {FingerRotationAmplitude_e.MiddleProximal, pose.middle.proximal},
                {FingerRotationAmplitude_e.MiddleIntermediate, pose.middle.intermediate},
                {FingerRotationAmplitude_e.MiddleDistal, pose.middle.distal},
                {FingerRotationAmplitude_e.IndexProximal, pose.index.proximal},
                {FingerRotationAmplitude_e.IndexIntermediate, pose.index.intermediate},
                {FingerRotationAmplitude_e.IndexDistal, pose.index.distal},
                {FingerRotationAmplitude_e.ThumbProximal, pose.thumb.proximal},
                {FingerRotationAmplitude_e.ThumbIntermediate, pose.thumb.intermediate},
                {FingerRotationAmplitude_e.ThumbDistal, pose.thumb.distal},
                {FingerRotationAmplitude_e.ThumbAbduction, 0.0f},
                {FingerRotationAmplitude_e.LittleSplay, 0.0f},
                {FingerRotationAmplitude_e.RingSplay, 0.0f},
                {FingerRotationAmplitude_e.MiddleSplay, 0.0f},
                {FingerRotationAmplitude_e.IndexSplay, 0.0f},
                {FingerRotationAmplitude_e.ThumbSplay, 0.0f}
            };
        }

        public FlexData(HandSides hand, float[] floats)
        {
            this.Hand = hand;
            this.Valid = true;
            _amplitudes = new Dictionary<FingerRotationAmplitude_e, float>
            {
                {FingerRotationAmplitude_e.ThumbProximal, floats[0]},
                {FingerRotationAmplitude_e.ThumbIntermediate, floats[1]},
                {FingerRotationAmplitude_e.ThumbDistal, floats[2]},
                {FingerRotationAmplitude_e.ThumbSplay, floats[3]},
                {FingerRotationAmplitude_e.IndexProximal, floats[4]},
                {FingerRotationAmplitude_e.IndexIntermediate, floats[5]},
                {FingerRotationAmplitude_e.IndexDistal, floats[6]},
                {FingerRotationAmplitude_e.IndexSplay, floats[7]},
                {FingerRotationAmplitude_e.MiddleProximal, floats[8]},
                {FingerRotationAmplitude_e.MiddleIntermediate, floats[9]},
                {FingerRotationAmplitude_e.MiddleDistal, floats[10]},
                {FingerRotationAmplitude_e.MiddleSplay, floats[11]},
                {FingerRotationAmplitude_e.RingProximal, floats[12]},
                {FingerRotationAmplitude_e.RingIntermediate, floats[13]},
                {FingerRotationAmplitude_e.RingDistal, floats[14]},
                {FingerRotationAmplitude_e.RingSplay, floats[15]},
                {FingerRotationAmplitude_e.LittleProximal, floats[16]},
                {FingerRotationAmplitude_e.LittleIntermediate, floats[17]},
                {FingerRotationAmplitude_e.LittleDistal, floats[18]},
                {FingerRotationAmplitude_e.LittleSplay, floats[19]},

                {FingerRotationAmplitude_e.ThumbAbduction, 0.0f}
            };
        }

        public float GetAmplitude(FingerRotationAmplitude_e section)
        {
            return _amplitudes[section];
        }

        public float this[FingerRotationAmplitude_e section] => this._amplitudes[section];
    }

    internal static class FingerRotationAmplitude_e_Ext
    {
        internal static int? ToMuscleIndex(this FingerRotationAmplitude_e bone, HandSides hand)
        {
            if (hand == HandSides.Right)
            {
                switch (bone)
                {
                    case FingerRotationAmplitude_e.IndexProximal:
                        return 79;
                    case FingerRotationAmplitude_e.IndexSplay:
                        return 80;
                    case FingerRotationAmplitude_e.IndexIntermediate:
                        return 81;
                    case FingerRotationAmplitude_e.IndexDistal:
                        return 82;
                    case FingerRotationAmplitude_e.MiddleProximal:
                        return 83;
                    case FingerRotationAmplitude_e.MiddleSplay:
                        return 84;
                    case FingerRotationAmplitude_e.MiddleIntermediate:
                        return 85;
                    case FingerRotationAmplitude_e.MiddleDistal:
                        return 86;
                    case FingerRotationAmplitude_e.RingProximal:
                        return 87;
                    case FingerRotationAmplitude_e.RingSplay:
                        return 88;
                    case FingerRotationAmplitude_e.RingIntermediate:
                        return 89;
                    case FingerRotationAmplitude_e.RingDistal:
                        return 90;
                    case FingerRotationAmplitude_e.LittleProximal:
                        return 91;
                    case FingerRotationAmplitude_e.LittleSplay:
                        return 92;
                    case FingerRotationAmplitude_e.LittleIntermediate:
                        return 93;
                    case FingerRotationAmplitude_e.LittleDistal:
                        return 94;
                    case FingerRotationAmplitude_e.ThumbProximal:
                        return 75;
                    case FingerRotationAmplitude_e.ThumbSplay:
                        return 76;
                    case FingerRotationAmplitude_e.ThumbIntermediate:
                        return 77;
                    case FingerRotationAmplitude_e.ThumbDistal:
                        return 78;
                    default:
                        return null;
                }
            }

            switch (bone)
            {
                case FingerRotationAmplitude_e.IndexProximal:
                    return 59;
                case FingerRotationAmplitude_e.IndexSplay:
                    return 60;
                case FingerRotationAmplitude_e.IndexIntermediate:
                    return 61;
                case FingerRotationAmplitude_e.IndexDistal:
                    return 62;
                case FingerRotationAmplitude_e.MiddleProximal:
                    return 63;
                case FingerRotationAmplitude_e.MiddleSplay:
                    return 64;
                case FingerRotationAmplitude_e.MiddleIntermediate:
                    return 65;
                case FingerRotationAmplitude_e.MiddleDistal:
                    return 66;
                case FingerRotationAmplitude_e.RingProximal:
                    return 67;
                case FingerRotationAmplitude_e.RingSplay:
                    return 68;
                case FingerRotationAmplitude_e.RingIntermediate:
                    return 69;
                case FingerRotationAmplitude_e.RingDistal:
                    return 70;
                case FingerRotationAmplitude_e.LittleProximal:
                    return 71;
                case FingerRotationAmplitude_e.LittleSplay:
                    return 72;
                case FingerRotationAmplitude_e.LittleIntermediate:
                    return 73;
                case FingerRotationAmplitude_e.LittleDistal:
                    return 74;
                case FingerRotationAmplitude_e.ThumbProximal:
                    return 55;
                case FingerRotationAmplitude_e.ThumbSplay:
                    return 56;
                case FingerRotationAmplitude_e.ThumbIntermediate:
                    return 57;
                case FingerRotationAmplitude_e.ThumbDistal:
                    return 58;
                default:
                    return null;
            }
        }

        internal static HumanBodyBones ToHumanBodyBones(this FingerRotationAmplitude_e finger, HandSides hand)
        {
            if (hand == HandSides.Right)
            {
                switch (finger)
                {
                    case FingerRotationAmplitude_e.IndexDistal:
                        return HumanBodyBones.RightIndexDistal;
                    case FingerRotationAmplitude_e.IndexIntermediate:
                        return HumanBodyBones.RightIndexIntermediate;
                    case FingerRotationAmplitude_e.IndexProximal:
                        return HumanBodyBones.RightIndexProximal;
                    case FingerRotationAmplitude_e.MiddleDistal:
                        return HumanBodyBones.RightMiddleDistal;
                    case FingerRotationAmplitude_e.MiddleIntermediate:
                        return HumanBodyBones.RightMiddleIntermediate;
                    case FingerRotationAmplitude_e.MiddleProximal:
                        return HumanBodyBones.RightMiddleProximal;
                    case FingerRotationAmplitude_e.RingDistal:
                        return HumanBodyBones.RightRingDistal;
                    case FingerRotationAmplitude_e.RingIntermediate:
                        return HumanBodyBones.RightRingIntermediate;
                    case FingerRotationAmplitude_e.RingProximal:
                        return HumanBodyBones.RightRingProximal;
                    case FingerRotationAmplitude_e.LittleDistal:
                        return HumanBodyBones.RightLittleDistal;
                    case FingerRotationAmplitude_e.LittleIntermediate:
                        return HumanBodyBones.RightLittleIntermediate;
                    case FingerRotationAmplitude_e.LittleProximal:
                        return HumanBodyBones.RightLittleProximal;
                    case FingerRotationAmplitude_e.ThumbDistal:
                        return HumanBodyBones.RightThumbDistal;
                    case FingerRotationAmplitude_e.ThumbIntermediate:
                        return HumanBodyBones.RightThumbIntermediate;
                    case FingerRotationAmplitude_e.ThumbProximal:
                        return HumanBodyBones.RightThumbProximal;
                    default:
                        Debug.Log("Unknown finger" + finger);
                        return HumanBodyBones.LastBone;
                }
            }
            else if (hand == HandSides.Left)
            {
                switch (finger)
                {
                    case FingerRotationAmplitude_e.IndexDistal:
                        return HumanBodyBones.LeftIndexDistal;
                    case FingerRotationAmplitude_e.IndexIntermediate:
                        return HumanBodyBones.LeftIndexIntermediate;
                    case FingerRotationAmplitude_e.IndexProximal:
                        return HumanBodyBones.LeftIndexProximal;
                    case FingerRotationAmplitude_e.MiddleDistal:
                        return HumanBodyBones.LeftMiddleDistal;
                    case FingerRotationAmplitude_e.MiddleIntermediate:
                        return HumanBodyBones.LeftMiddleIntermediate;
                    case FingerRotationAmplitude_e.MiddleProximal:
                        return HumanBodyBones.LeftMiddleProximal;
                    case FingerRotationAmplitude_e.RingDistal:
                        return HumanBodyBones.LeftRingDistal;
                    case FingerRotationAmplitude_e.RingIntermediate:
                        return HumanBodyBones.LeftRingIntermediate;
                    case FingerRotationAmplitude_e.RingProximal:
                        return HumanBodyBones.LeftRingProximal;
                    case FingerRotationAmplitude_e.LittleDistal:
                        return HumanBodyBones.LeftLittleDistal;
                    case FingerRotationAmplitude_e.LittleIntermediate:
                        return HumanBodyBones.LeftLittleIntermediate;
                    case FingerRotationAmplitude_e.LittleProximal:
                        return HumanBodyBones.LeftLittleProximal;
                    case FingerRotationAmplitude_e.ThumbDistal:
                        return HumanBodyBones.LeftThumbDistal;
                    case FingerRotationAmplitude_e.ThumbIntermediate:
                        return HumanBodyBones.LeftThumbIntermediate;
                    case FingerRotationAmplitude_e.ThumbProximal:
                        return HumanBodyBones.LeftThumbProximal;
                    
                    default:
                        Debug.Log("Unknown finger" + finger);
                        return HumanBodyBones.LastBone;
                }
            }

            Debug.Log("Finger not found");
            return HumanBodyBones.LastBone;
        }


        internal static float GetMaximumAmplitude(this FingerRotationAmplitude_e bone)
        {
            float maximumAmplitude = 0;
            switch (bone)
            {
                case FingerRotationAmplitude_e.ThumbIntermediate:
                    maximumAmplitude = 55;
                    break;
                case FingerRotationAmplitude_e.ThumbProximal:
                    maximumAmplitude = 30;
                    break;
                case FingerRotationAmplitude_e.ThumbDistal:
                    maximumAmplitude = 80;
                    break;
                case FingerRotationAmplitude_e.IndexDistal:
                case FingerRotationAmplitude_e.LittleDistal:
                case FingerRotationAmplitude_e.MiddleDistal:
                case FingerRotationAmplitude_e.RingDistal:
                    maximumAmplitude = 90;
                    break;
                case FingerRotationAmplitude_e.IndexIntermediate:
                case FingerRotationAmplitude_e.MiddleIntermediate:
                case FingerRotationAmplitude_e.RingIntermediate:
                case FingerRotationAmplitude_e.LittleIntermediate:
                    maximumAmplitude = 100;
                    break;
                case FingerRotationAmplitude_e.IndexProximal:
                case FingerRotationAmplitude_e.MiddleProximal:
                case FingerRotationAmplitude_e.RingProximal:
                case FingerRotationAmplitude_e.LittleProximal:
                    maximumAmplitude = 90;
                    break;
                case FingerRotationAmplitude_e.ThumbAbduction:
                    maximumAmplitude = 0;
                    break;
            }
            return maximumAmplitude;
        }

        
    }
}