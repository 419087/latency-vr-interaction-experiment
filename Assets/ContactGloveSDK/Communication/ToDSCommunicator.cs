using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using uOSC;

namespace ContactGloveSDK
{
    public class ToDSCommunicator : MonoBehaviour
    {
        public const int VERSION = 1;
        
        private static ToDSCommunicator _toDSCommunicator;
        
        private static readonly string _toDSCommunicatorObjectName = "ToDSCommunicator";
        
        private readonly string _ipAddress = "127.0.0.1";
        private readonly int _sendOscPort = 25790;
        [SerializeField][ReadOnly] private int defaultReceiveOscPort = 25788;
        private int receiveOscPort = -1;
        
        private uOscClient _oscClient;
        private uOscServer _oscServer;
        
        public static ToDSCommunicator GetInstance()
        {
            if (_toDSCommunicator == null)
            {
                // find objects in scene
                var go = GameObject.Find(_toDSCommunicatorObjectName);
                if (go == null)
                {
                    go = new GameObject(_toDSCommunicatorObjectName);
                    _toDSCommunicator = go.AddComponent<ToDSCommunicator>();
                }
                else
                {
                    _toDSCommunicator = go.GetComponent<ToDSCommunicator>();
                }
            }

            return _toDSCommunicator;
        }

        private static int FindFreePort(int defaultPort)
        {
            if (IsPortAvailable(defaultPort))
            {
                Debug.Log($"Found free port: {defaultPort}");
                return defaultPort;
            }
            
            using (var udpClient = new UdpClient(0))
            {
                var localEndPoint = (IPEndPoint) udpClient.Client.LocalEndPoint;
                var port = localEndPoint.Port;
                Debug.Log($"Found free port: {port}");
                return port;
            }
        }
        
        private static bool IsPortAvailable(int port)
        {
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.ExclusiveAddressUse = true;
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
            finally
            {
                if (socket != null)
                    socket.Close();
            }
        }
        
        private void Awake()
        {
            if (_toDSCommunicator == null)
            {
                _toDSCommunicator = this;
            }
            else if (_toDSCommunicator != this)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            
            var oscClient = gameObject.GetComponent<uOscClient>() ?? gameObject.AddComponent<uOscClient>();
            var oscServer = gameObject.GetComponent<uOscServer>() ?? gameObject.AddComponent<uOscServer>();
            
            receiveOscPort = FindFreePort(defaultReceiveOscPort);

            oscClient.port = _sendOscPort;
            oscClient.address = _ipAddress;

            oscServer.port = receiveOscPort;
            oscServer.onDataReceived.AddListener(this.OnDataReceived);

            _oscClient = oscClient;
            _oscServer = oscServer;
        }
        
        private float _lastUpdateTime = 0;
        private float _updateInterval = 1;
        private void Update()
        {
            if (Time.time - _lastUpdateTime > _updateInterval)
            {
                _lastUpdateTime = Time.time;
                Connect();
            }
        }
        
        private void OnDestroy()
        {
            Disconnect();
        }
        
        private void OnApplicationQuit()
        {
            Disconnect();
        }
        
        public void AddReceiver(GlovePriority priority, IGloveDataReceiver receiver)
        {
            if (priority == GlovePriority.None)
            {
                Debug.LogWarning($"Invalid priority: {priority}");
                return;
            }

            DeviceStatusManager.RegisterHandler(priority, receiver);
        }

        public IGloveDataSender GetSender(GlovePriority priority)
        {
            return new OscGloveDataSender(priority, this);
        }

        public void Send(string address, params object[] values)
        {
            _oscClient.Send(address, values);
        }
        
        private void OnDataReceived(Message message)
        {
            this._lastUpdateTime = Time.time;
            
            var data = message.values;
            if (data.Length < 1)
            {
                Debug.LogError("Receive illegal data");
                return;
            }
            if (data[0] is not int)
            {
                Debug.LogError("Receive illegal version");
                return;
            }
            if (data[0] is int version && version != VERSION)
            {
                Debug.LogError($"Receive illegal version: {version}");
                return;
            }
            
            var payload = data.Skip(1).ToArray();
            
            if (message.address == OscModes.ReceiveDevice.ToIpAddress())
            {
                OnDeviceDataReceived(payload);
            }
            else if (message.address == OscModes.ReceiveFingerFlex.ToIpAddress())
            {
                OnFingerFlexDataReceived(payload);
            }
            else if (message.address == OscModes.ReceiveFingerQuat.ToIpAddress())
            {
                OnFingerQuatDataReceived(payload);
            }
            else if (message.address == OscModes.ReceiveWristQuat.ToIpAddress())
            {
                OnWristQuatDataReceived(payload);
            }
            else if (message.address == OscModes.ReceiveControllerInput.ToIpAddress())
            {
                OnControllerDataReceived(payload);
            }
        }
        
        private string ToStr(object datum)
        {
            Assert.IsTrue(datum is string);
            return (string)datum;
        }
        
        private bool ToBool(object datum)
        {
            Assert.IsTrue(datum is int);
            return (int)datum == 1;
        }
        
        private int ToInt(object datum)
        {
            Assert.IsTrue(datum is int);
            return (int)datum;
        }
        
        private float ToFloat(object datum)
        {
            Assert.IsTrue(datum is float);
            return (float)datum;
        }

        private void OnDeviceDataReceived(object[] data)
        {
            const int unitLength = 12;
            if (data.Length % unitLength != 0)
            {
                Debug.LogError("Receive illegal data on device data");
                return;
            }
            for (var i = 0; i < data.Length; i += unitLength)
            {
                var unitData = data.Skip(i).Take(unitLength).ToArray();
                var id = ToStr(unitData[0]);
                var property = new DeviceStatus
                {
                    isMain = ToBool(unitData[1]),
                    deviceType = ToInt(unitData[2]),
                    name = ToStr(unitData[3]),
                    color = ToInt(unitData[4]),
                    ping = ToFloat(unitData[5]),
                    isLeftConnected = ToBool(unitData[6]),
                    leftBattery = ToInt(unitData[7]),
                    leftPing = ToFloat(unitData[8]),
                    isRightConnected = ToBool(unitData[9]),
                    rightBattery = ToInt(unitData[10]),
                    rightPing = ToFloat(unitData[11]),
                };
                DeviceStatusManager.SetStatus(id, property);
            }
        }

        private void OnFingerFlexDataReceived(object[] data)
        {
            const int unitLength = 2 + 5 * 4;
            if (data.Length != unitLength)
            {
                Debug.LogError("Receive illegal data on finger flex");
                return;
            }
            var id = ToStr(data[0]);
            var isLeft = ToBool(data[1]);
            var floats = data.Skip(2).Select(x => (float) x).ToArray();
            var flexData = new FlexData(isLeft ? HandSides.Left : HandSides.Right, floats);

            if (!flexData.Valid || id == null) return;

            DeviceStatusManager.HandleById(id, handler =>
            {
                handler.OnFlexDataReceived(flexData);
            });
        }

        private void OnFingerQuatDataReceived(object[] data)
        {
            
        }

        private void OnWristQuatDataReceived(object[] data)
        {
            const int unitLength = 2 + 4;
            if (data.Length != unitLength)
            {
                Debug.LogError("Receive illegal data on finger flex");
                return;
            }

            var id = ToStr(data[0]);
            var isLeft = ToBool(data[1]);
            var w = (float) data[2];
            var x = (float) data[3];
            var y = (float) data[4];
            var z = (float) data[5];
            var rotation = new Quaternion(x, y, z, w);
            var wristPose = new WristPose(isLeft ? HandSides.Left : HandSides.Right, rotation);
            
            DeviceStatusManager.HandleById(id, handler =>
            {
                handler.OnWristPoseReceived(wristPose);
            });
        }

        private void OnControllerDataReceived(object[] data)
        {
            const int unitLength = 2 + 6 + 7;
            if (data.Length != unitLength)
            {
                Debug.LogError($"Receive illegal data on controller: expected {unitLength}, received {data.Length}");
                return;
            }

            var id = ToStr(data[0]);
            var isLeft = ToBool(data[1]);
            var intValues = data.Skip(2).Take(6).Select(ToInt).ToArray();
            var floatValues = data.Skip(8).Select(x => (float)x).ToArray();
            var controllerData =
                new ControllerData(isLeft ? HandSides.Left : HandSides.Right, intValues, floatValues);

            if (!controllerData.Valid || id == null) return;
            
            DeviceStatusManager.HandleById(id, handler =>
            {
                handler.OnControllerDataReceived(controllerData);
            });
        }
        
        internal void SendVibration(string id, HandSides hand, float amplitude, float frequency, float duration)
        {
            var address = $"/DS/HC/{id}/Haptics/Body";
            Send(address, VERSION, hand == HandSides.Left ? 1 : 0, frequency, amplitude, duration);
        }
        
        internal void SendHaptics(string id, byte[] data)
        {
            var address = $"/DS/HC/{id}/Haptics/Finger";
            
            var leftThumb = data[0] & 0b0000_1111;
            var leftIndex = data[0] >> 4;
            var leftMiddle = data[1] & 0b0000_1111;
            var leftRing = data[1] >> 4;
            var leftBack = data[2] & 0b0000_1111;
            var leftForce = data[2] >> 4;
            
            var rightThumb = data[3] & 0b0000_1111;
            var rightIndex = data[3] >> 4;
            var rightMiddle = data[4] & 0b0000_1111;
            var rightRing = data[4] >> 4;
            var rightBack = data[5] & 0b0000_1111;
            var rightForce = data[5] >> 4;

            Send(address, VERSION, 1, leftThumb, leftIndex, leftMiddle, leftRing, leftForce);
            Send(address, VERSION, 0, rightThumb, rightIndex, rightMiddle, rightRing, rightForce);
        }
        
        public void Connect()
        {
            var address = $"/DS/HC/Connect";
            Send(address, receiveOscPort);
        }
        
        public void Disconnect()
        {
            var address = $"/DS/HC/Disconnect";
            Send(address, receiveOscPort);
        }
    }
}