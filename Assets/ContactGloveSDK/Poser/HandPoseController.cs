using UnityEngine;

namespace ContactGloveSDK
{
    public class HandPoseController : MonoBehaviour
    {
        public HandSides Side;

        private Vector3 initialWristPosition;
        private bool isAutoUpdate = true;

        // the transform of hand model
        public Transform HandModel;

        void Start()
        {
        }

        void Update()
        {
            if(isAutoUpdate)
            {
                SetPoseFromTracker();
            }
        }

        /// <summary>
        /// Set hand pose from vive tracker
        /// </summary>
        public void SetPoseFromTracker()
        {
            HandModel.position = GetPositionFromViveTracker();
            HandModel.rotation = GetRotationFromViveTracker();
        }

        private static readonly Vector3 RightWristPosOffset = new Vector3(0.045f, 0.003f, -0.15f);
        private static readonly Vector3 LeftWristPosOffset = new Vector3(-0.045f, 0.003f, -0.15f);
        public Vector3 GetPositionFromViveTracker()
        {
            if (this.Side == HandSides.Left)
            {
                return this.transform.TransformPoint(LeftWristPosOffset);
            }
            else
            {
                return this.transform.TransformPoint(RightWristPosOffset);
            }
        }

        private static readonly Quaternion RightWristRotOffset = Quaternion.Euler(-40.0f, 0.0f, -70.0f);
        private static readonly Quaternion LeftWristRotOffset = Quaternion.Euler(140.0f, 0.0f, -70.0f);
        public Quaternion GetRotationFromViveTracker()
        {
            if (this.Side == HandSides.Left)
            {
                return this.transform.rotation * LeftWristRotOffset;
            }
            else
            {
                return this.transform.rotation * RightWristRotOffset;
            }
        }
    }
}