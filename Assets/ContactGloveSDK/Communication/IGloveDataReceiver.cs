namespace ContactGloveSDK
{
    public interface IGloveDataReceiver
    {
        void OnFlexDataReceived(FlexData data);
        void OnControllerDataReceived(ControllerData data);
        void OnWristPoseReceived(WristPose pose);
    }
}