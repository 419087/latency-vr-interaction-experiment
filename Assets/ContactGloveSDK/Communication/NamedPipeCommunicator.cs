using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ContactGloveSDK
{
    
    using ContactGloveDataArray = ContactGloveSDK.NamedPipe.ContactGloveDataArray;
    using VibrationDataWithId = ContactGloveSDK.NamedPipe.VibrationDataWithId;
    using HapticsDataWithId = ContactGloveSDK.NamedPipe.HapticsDataWithId;

    public class NamedPipeCommunicator
    {
        private static NamedPipeCommunicator _namedPipeCommunicator;

        public static NamedPipeCommunicator GetInstance()
        {
            return _namedPipeCommunicator ??= new NamedPipeCommunicator();
        }

        private readonly NamedPipeReceiver<ContactGloveDataArray> _ContactGloveDataReceiver;

        private readonly NamedPipeSender<VibrationDataWithId> _LeftVibrationSender;
        private readonly NamedPipeSender<VibrationDataWithId> _RightVibrationSender;
        private readonly NamedPipeSender<HapticsDataWithId> _HapticsSender;

        private readonly Dictionary<GlovePriority, List<IGloveDataReceiver>> _handlers;

        private void OnContactGloveDataReceived(ContactGloveDataArray data)
        {
            foreach (var contactGloveData in data.data)
            {
                var id = contactGloveData.id;
                DeviceStatusManager.RegisterId(id);
                var priority = DeviceStatusManager.GetPriority(id);
                
                if (!_handlers.TryGetValue(priority, out var handlers))
                {
                    Debug.LogWarning($"No handler for priority {priority}");
                    continue;
                }
                
                foreach (var handler in handlers)
                {
                    var leftFlex = new FlexData(HandSides.Left, contactGloveData.left_hand_pose);
                    var rightFlex = new FlexData(HandSides.Right, contactGloveData.right_hand_pose);
                    var leftController = new ControllerData(HandSides.Left, contactGloveData.left_controller);
                    var rightController = new ControllerData(HandSides.Right, contactGloveData.right_controller);
                    var leftWristPose = new WristPose(HandSides.Left, contactGloveData.left_wrist_pose);
                    var rightWristPose = new WristPose(HandSides.Right, contactGloveData.right_wrist_pose);
                    
                    handler.OnFlexDataReceived(leftFlex);
                    handler.OnFlexDataReceived(rightFlex);
                    handler.OnControllerDataReceived(leftController);
                    handler.OnControllerDataReceived(rightController);
                    handler.OnWristPoseReceived(leftWristPose);
                    handler.OnWristPoseReceived(rightWristPose);
                }
            }
        }
        
        private static string GetPipeName(string role)
        {
            return $"diver-x.jp\\sdk.contactglove\\{role}";
        }
        
        private NamedPipeCommunicator()
        {
            _ContactGloveDataReceiver = new NamedPipeReceiver<ContactGloveDataArray>(
                GetPipeName("glove-data"), OnContactGloveDataReceived);

            _LeftVibrationSender = new NamedPipeSender<VibrationDataWithId>(
                ".", GetPipeName("vibration-left"));
            _RightVibrationSender = new NamedPipeSender<VibrationDataWithId>(
                ".", GetPipeName("vibration-right"));
            _HapticsSender = new NamedPipeSender<HapticsDataWithId>(
                ".", GetPipeName("haptics"));

            _handlers = new Dictionary<GlovePriority, List<IGloveDataReceiver>>();

            foreach (GlovePriority priority in Enum.GetValues(typeof(GlovePriority)))
            {
                if (priority == GlovePriority.None)
                {
                    continue;
                }
                
                _handlers[priority] = new List<IGloveDataReceiver>();
            }

            _ContactGloveDataReceiver.Run();
        }
        
        public void AddReceiver(GlovePriority priority, IGloveDataReceiver receiver)
        {
            if (priority == GlovePriority.None)
            {
                Debug.LogWarning($"Invalid priority: {priority}");
                return;
            }

            _handlers[priority].Add(receiver);
        }
        
        public IGloveDataSender GetSender(GlovePriority priority)
        {
            return new NamedPipeGloveDataSender(priority, this);
        }
        
        private static VibrationDataWithId CreateVibrationData(string id, float amplitude, float frequency, float duration)
        {
            return new VibrationDataWithId
            {
                id = id,
                vibration = new NamedPipe.VibrationData
                {
                    amplitude = amplitude,
                    frequency = frequency,
                    duration = duration
                }
            };
        }
        
        private static HapticsDataWithId CreateHapticsData(string id, byte[] data)
        {
            return new HapticsDataWithId
            {
                id = id,
                haptics = new NamedPipe.HapticsData
                {
                    data = data
                }
            };
        }
        
        internal void SendLeftVibration(string id, float amplitude, float frequency, float duration)
        {
            var data = CreateVibrationData(id, amplitude, frequency, duration);
            UniTask.RunOnThreadPool(() => _LeftVibrationSender.Send(data));
        }
        
        internal void SendRightVibration(string id, float amplitude, float frequency, float duration)
        {
            var data = CreateVibrationData(id, amplitude, frequency, duration);
            UniTask.RunOnThreadPool(() => _RightVibrationSender.Send(data));
        }
        
        internal void SendHaptics(string id, byte[] data)
        {
            var hapticsData = CreateHapticsData(id, data);
            UniTask.RunOnThreadPool(() => _HapticsSender.Send(hapticsData));
        }
        
        public void StopListening()
        {
            _ContactGloveDataReceiver.Close();
            _LeftVibrationSender.Close();
            _RightVibrationSender.Close();
            _HapticsSender.Close();
            Debug.Log("Stop listening");
        }
    }
}