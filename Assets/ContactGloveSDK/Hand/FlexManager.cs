// using HandPhysicsToolkit.Utils;
using UnityEngine;

namespace ContactGloveSDK
{
    internal class FlexManager : IFlexManager
    {
        private readonly IFlexObjectHandler flexObjectHandler;

        /// <summary>
        /// Whether to use Animator or not. If true, fingerTransforms will be ignored.
        /// </summary>
        private bool useAnimator;

        /// <summary>
        /// default avatar which will be used to calculate hand only model's finger rotation
        /// </summary>
        private GameObject avatarModelDefault;

        /// <summary>
        /// Use handPhysics to calculate human pose?
        /// </summary>
        private bool useHandPhysics;

        /// <summary>
        /// Amplitude of previous frame finger rotation. Used to calculate RC filter
        /// </summary>
        private readonly float[,] previousFingerRotationAmplitude =
            new float [DataLength.HandLength, DataLength.FingerRot];

        private readonly Quaternion[] fingerRotationOffsetLeft = new Quaternion[DataLength.FingerRot];
        private readonly Quaternion[] fingerRotationOffsetRight = new Quaternion[DataLength.FingerRot];


        internal FlexManager(
            IFlexObjectHandler flexObjectHandler,
            bool useAnimator,
            bool useHandPhysics)

        {
            this.flexObjectHandler = flexObjectHandler;
            this.useAnimator = useAnimator;
            this.useHandPhysics = useHandPhysics;
        }

        public void InitializeFingerRotation(HandSides hand)
        {
            var fingerRotationOffset = (hand == HandSides.Left ? fingerRotationOffsetLeft : fingerRotationOffsetRight);

            // Initialize bend and straight wrist transforms
            for (int i = 0; i < DataLength.FingerRot; i++)
            {
                FingerRotationAmplitude_e finger_e = (FingerRotationAmplitude_e)i;
                if (flexObjectHandler.GetFingerTransform(hand, finger_e) == null)
                {
                    continue;
                }

                fingerRotationOffset[i] = flexObjectHandler.GetFingerTransform(hand, finger_e).localRotation;
            }
        }

        public void SetFingerRotationFromAmplitude(HandSides hand)
        {
            // i is the index of the finger rotation amplitude
            // In raw-transform mode, bones without transforms will be ignored by SetBoneRotation.
            for (int i = 0; i < DataLength.FingerRot; i++)
            {
                SetBoneRotation((FingerRotationAmplitude_e)i, flexObjectHandler.GetFingerRotationAmplitude(hand,(FingerRotationAmplitude_e)i), hand);
            }
        }

        /// <summary>
        /// Calculate Quaternion value of finger from rotationAmplitude
        /// </summary>
        /// <param name="rotationAmplitude"> Amplitude received from HPE</param>
        /// <returns></returns>
        public Quaternion FingerRotationFromAmplitude(FingerRotationAmplitude_e bone, float rotationAmplitude,
            HandSides hand)
        {
            Quaternion offset = Quaternion.identity;
            
            // Thumb abduction has different offset
            if (bone == FingerRotationAmplitude_e.ThumbAbduction)
            {
                float thumbAbductionAngle = MainTool.MapValue(rotationAmplitude, 0, 1, -25, 0);
                offset = Quaternion.AngleAxis(-thumbAbductionAngle, Vector3.up);
            }

            float maximumAmplitude = bone.GetMaximumAmplitude();
            rotationAmplitude = Mathf.Clamp(rotationAmplitude, 0, 1);

            if (hand == HandSides.Left)
            {
                return fingerRotationOffsetLeft[(int)bone] *
                       Quaternion.AngleAxis(-maximumAmplitude * rotationAmplitude, Vector3.forward) * offset;
            }
            else
            {
                return fingerRotationOffsetRight[(int)bone] *
                       Quaternion.AngleAxis(-maximumAmplitude * rotationAmplitude, Vector3.forward) * offset;
            }
        }

        public void SetBoneRotation(FingerRotationAmplitude_e bone, float fingerRotAmplitude, HandSides hand)
        {
            if (useAnimator)
            {
                SetMuscle(bone, fingerRotAmplitude, hand);
            }
            // use raw bone transform
            else
            {
                Transform fingerTransform = flexObjectHandler.GetFingerTransform(hand, bone);
                if (fingerTransform == null)
                {
                    return;
                }

                Quaternion rot = FingerRotationFromAmplitude(bone, fingerRotAmplitude, hand);
                fingerTransform.localRotation = rot;
            }
        }

        public void SetMuscle(FingerRotationAmplitude_e bone, float rotAmplitude, HandSides hand)
        {
            var humanPose = new HumanPose();
            flexObjectHandler.GetHumanPose(ref humanPose);

            int? muscleIndex = bone.ToMuscleIndex(hand);

            if (muscleIndex != null)
            {
                humanPose.muscles[muscleIndex.Value] = rotAmplitude;
                flexObjectHandler.SetHumanPose(ref humanPose);
            }
            else
            {
                Debug.LogError("Unknown Finger Bone " + bone);
            }
        }
    }
}