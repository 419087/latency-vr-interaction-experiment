using System;
using System.Runtime.InteropServices;

namespace ContactGloveSDK
{
    namespace NamedPipe
    {
        [Serializable]
        public class ContactGloveDataArray
        {
            public SdkData[] data;
        }

        [Serializable]
        public class SdkData
        {
            public string id;
            public HandPose left_hand_pose;
            public HandPose right_hand_pose;
            public WristPose left_wrist_pose;
            public WristPose right_wrist_pose;
            public ControllerInput left_controller;
            public ControllerInput right_controller;
        }
        
        [Serializable]
        public class VibrationDataWithId
        {
            public string id;
            public VibrationData vibration;
        }
        
        [Serializable]
        public class VibrationData
        {
            public float frequency;
            public float amplitude;
            public float duration;
        }
        
        [Serializable]
        public class HapticsDataWithId
        {
            public string id;
            public HapticsData haptics;
        }
        
        [Serializable]
        public class HapticsData
        {
            public byte[] data;
        }

        [Serializable]
        public class ControllerInput
        {
            public bool a_pressed;
            public bool b_pressed;
            public bool sys_pressed;
            public bool joystick_pressed;
            public bool trackpad_pressed;
            
            public bool a_touched;
            public bool b_touched;
            public bool sys_touched;
            public bool joystick_touched;
            public bool trackpad_touched;
            
            public float trigger;
            public float grip_value;
            public float grip_force;
            public float joystick_x;
            public float joystick_y;
            public float trackpad_x;
            public float trackpad_y;
        }
        
        [Serializable]
        public class WristPose
        {
            public float x;
            public float y;
            public float z;
            public float w;
        }
        
        [Serializable]
        public class HandPose
        {
            public FingerPose thumb;
            public FingerPose index;
            public FingerPose middle;
            public FingerPose ring;
            public FingerPose little;
        }
        
        [Serializable]
        public class FingerPose
        {
            public float proximal;
            public float intermediate;
            public float distal;
        }
    }

}