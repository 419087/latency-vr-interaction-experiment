namespace ContactGloveSDK
{
    internal class OscGloveDataSender : IGloveDataSender
    {
        private readonly GlovePriority _priority;
        private readonly ToDSCommunicator _toDSCommunicator;
        private string _id = null;

        public OscGloveDataSender(string id, ToDSCommunicator toDSCommunicator)
        {
            _id = id;
            _toDSCommunicator = toDSCommunicator;
        }
        
        public OscGloveDataSender(GlovePriority priority, ToDSCommunicator toDSCommunicator)
        {
            _priority = priority;
            _toDSCommunicator = toDSCommunicator;
        }

        public void SendVibration(HandSides hand, float amplitude, float frequency, float duration)
        {
            if (_id == null)
            {
                _id = DeviceStatusManager.GetId(_priority);
                if (_id == null)
                {
                    return;
                }
            }
            
            _toDSCommunicator.SendVibration(_id, hand, amplitude, frequency, duration);
        }

        public void SendHaptics(byte[] data)
        {
            if (_id == null)
            {
                _id = DeviceStatusManager.GetId(_priority);
                if (_id == null)
                {
                    return;
                }
            }
            
            _toDSCommunicator.SendHaptics(_id, data);
        }
    }
    
    internal class NamedPipeGloveDataSender : IGloveDataSender
    {
        private readonly GlovePriority _priority;
        private readonly NamedPipeCommunicator _namedPipeCommunicator;
        private string _id = null;

        public NamedPipeGloveDataSender(string id, NamedPipeCommunicator namedPipeCommunicator)
        {
            _id = id;
            _namedPipeCommunicator = namedPipeCommunicator;
        }
        
        public NamedPipeGloveDataSender(GlovePriority priority, NamedPipeCommunicator namedPipeCommunicator)
        {
            _priority = priority;
            _namedPipeCommunicator = namedPipeCommunicator;
        }

        public void SendVibration(HandSides hand, float amplitude, float frequency, float duration)
        {
            if (_id == null)
            {
                _id = DeviceStatusManager.GetId(_priority);
                if (_id == null)
                {
                    return;
                }
            }

            if (hand == HandSides.Left)
            {
                _namedPipeCommunicator.SendLeftVibration(_id, amplitude, frequency, duration);
            }
            else
            {
                _namedPipeCommunicator.SendRightVibration(_id, amplitude, frequency, duration);
            }
        }

        public void SendHaptics(byte[] data)
        {
            if (_id == null)
            {
                _id = DeviceStatusManager.GetId(_priority);
                if (_id == null)
                {
                    return;
                }
            }

            _namedPipeCommunicator.SendHaptics(_id, data);
        }
    }
}