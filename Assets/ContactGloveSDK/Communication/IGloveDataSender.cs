
namespace ContactGloveSDK
{
    public interface IGloveDataSender
    {
        void SendVibration(HandSides hand, float amplitude, float frequency, float duration);
        void SendHaptics(byte[] data);
    }
}