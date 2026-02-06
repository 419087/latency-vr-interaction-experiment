using UnityEngine;

namespace ContactGloveSDK
{
    // Definition of send/receive modes
    // 0 ~ 9: send; 10 ~ : receive
    public enum OscModes
    {
        SendSetup = 0,
        SendCollision = 1,
        SendVibration = 2,
        
        ReceiveDevice = 11,
        ReceiveFingerFlex = 12,
        ReceiveFingerQuat = 13,
        ReceiveWristQuat = 14,
        ReceiveControllerInput = 15,
    }

    internal static class OscModesExt
    {
        internal static string ToIpAddress(this OscModes mode)
        {
            switch (mode)
            {
                // case OscModes.SendCollision:
                //     return "/DivingStation/CollisionSignal";
                // case OscModes.SendVibration:
                //     return "/DivingStation/HapticVibration";
                
                case OscModes.ReceiveDevice:
                    return "/DS/HC/Device";
                case OscModes.ReceiveFingerFlex:
                    return "/DS/HC/Hand";
                case OscModes.ReceiveFingerQuat:
                    return "/DS/HC/HandQuat";
                case OscModes.ReceiveWristQuat:
                    return "/DS/HC/Wrist";
                case OscModes.ReceiveControllerInput:
                    return "/DS/HC/Controller";
            }

            Debug.LogError("Switch is not exhaustive");
            return "";
        }
        
        internal static int Length(this OscModes mode)
        {
            switch (mode)
            {
                case OscModes.SendCollision:
                    return 1;
                case OscModes.SendVibration:
                    return 4;
                
                case OscModes.ReceiveDevice:
                    return 9;
                case OscModes.ReceiveFingerFlex:
                    return 2 + 5 * 4;
                case OscModes.ReceiveFingerQuat:
                    return 2 + 5 * 3 * 4;
                case OscModes.ReceiveWristQuat:
                    return 2 + 4;
                case OscModes.ReceiveControllerInput:
                    return 2 + 5 + 6;
            }

            Debug.LogError("Switch is not exhaustive");
            return -1;
        }
    }
}