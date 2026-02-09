using UnityEngine;

namespace ContactGloveSDK
{
    public class WristPose
    {
        private readonly Quaternion _rotation;
        private readonly HandSides _handSide;

        public Quaternion Rotation => _rotation;
        public HandSides HandSide => _handSide;

        public WristPose(HandSides handSide, Quaternion rotation)
        {
            _handSide = handSide;
            _rotation = rotation;
        }

        public WristPose(HandSides handSide, NamedPipe.WristPose wristPose)
        {
            _handSide = handSide;
            _rotation = new Quaternion(wristPose.x, wristPose.y, wristPose.z, wristPose.w);
        }
    }
}