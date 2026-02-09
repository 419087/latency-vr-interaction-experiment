using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using uOSC;

namespace ContactGloveSDK
{
    public class ContactGloveManager : MonoBehaviour, IFlexObjectHandler, IContactGloveManager, IGloveDataReceiver
    {
        private IGloveDataSender _sender;

        private HumanPoseHandler humanPoseHandler;

        private IFingerManager fingerManager;

        private IFlexManager flexManager;

        private ControllerInput _controllerInput;
        
        // private NamedPipeCommunicator namedPipeCommunicator;

        [SerializeField] private GlovePriority priority = GlovePriority.None;

        [SerializeField] private Animator animator;

        [SerializeField] private GameObject collisionPrefab;

        /// <summary>
        /// Amplitude of finger rotation. 0: 0 degrees, 1: 90 degrees
        /// Index: FingerRotationAmplitudeRight
        /// </summary>
        private readonly float[,] fingerRotationAmplitude = new float[DataLength.HandLength, DataLength.FingerRot];

        /// <summary>
        /// Whether to use Animator or not. If true, fingerTransforms will be ignored.
        /// </summary>
        [SerializeField, Header("Flex"), Space(10),]
        private bool useAnimator = false;

        /// <summary>
        /// Use HandPhysics to calculate human pose?
        /// </summary>
        [SerializeField]
        private bool useHandPhysics = false;
        
        /// <summary>
        /// Attach haptics collider to each fingers?
        /// </summary>
        [SerializeField] 
        private bool attachHapticsCollider = false;

        /// <summary>
        /// Finger Transforms. Used when useAnimator is false
        /// </summary>
        [SerializeField, EnumIndex(typeof(FingerRotationAmplitude_e))]
        private Transform[] fingerTransformsRight = new Transform[DataLength.FingerBone];

        [SerializeField, EnumIndex(typeof(FingerRotationAmplitude_e))]
        private Transform[] fingerTransformsLeft = new Transform[DataLength.FingerBone];

        [SerializeField, EnumIndex(typeof(FingerHandPhysics_e))]
        private Transform[] fingerHandPhysicsTransformsLeft = new Transform[DataLength.FingerHandPhysics];

        [SerializeField, EnumIndex(typeof(FingerHandPhysics_e))]
        private Transform[] fingerHandPhysicsTransformsRight = new Transform[DataLength.FingerHandPhysics];

        private readonly HapticsManager hapticsManager = new HapticsManager();
        
        private WristPose _leftWristPose = new WristPose(HandSides.Left, Quaternion.identity);
        private WristPose _rightWristPose = new WristPose(HandSides.Right, Quaternion.identity);

        private class TmpColliderObjectHandler : IColliderObjectHandler
        {
            private ContactGloveManager manager;

            public TmpColliderObjectHandler(ContactGloveManager manager)
            {
                this.manager = manager;
            }
            
            public GameObject CreateNewCollisionObject(Vector3 position)
            {
                return Instantiate(manager.collisionPrefab, position, Quaternion.identity);
            }

            public Transform GetBoneTransform(HumanBodyBones bone) => manager.GetBoneTransform(bone);

            public Transform GetFingerTransform(HandSides hand, FingerRotationAmplitude_e bone) => manager.GetFingerTransform(hand, bone);

            public Transform GetHandPhysicsFingerTransform(HandSides hand, FingerHandPhysics_e bone)
            {
                if (hand == HandSides.Right)
                    return manager.fingerHandPhysicsTransformsRight[(int)bone];
                return manager.fingerHandPhysicsTransformsLeft[(int)bone];
            }
        }

        private void Awake()
        {
            fingerManager = new FingerManager(new TmpColliderObjectHandler(this), useAnimator, useHandPhysics);

            flexManager = new FlexManager(
                this,
                useAnimator,
                useHandPhysics
            );
            
            var toDsCommunicator = ToDSCommunicator.GetInstance();
            toDsCommunicator.AddReceiver(priority, this);
            _sender = toDsCommunicator.GetSender(priority);

            // namedPipeCommunicator = NamedPipeCommunicator.GetInstance();
            // namedPipeCommunicator.AddReceiver(priority, this);
            // _sender = namedPipeCommunicator.GetSender(priority);

            _controllerInput = new ControllerInput();

            if (attachHapticsCollider)
            {
                fingerManager.MapCollider();
            }
        }

        private void Start()
        {
            if (!useAnimator)
            {
                flexManager.InitializeFingerRotation(HandSides.Left);
                flexManager.InitializeFingerRotation(HandSides.Right);
            }
            else
            {
                humanPoseHandler = new HumanPoseHandler(animator.avatar, animator.transform);
            }
        }
        
        private void Update()
        {
            flexManager.SetFingerRotationFromAmplitude(HandSides.Left);
            flexManager.SetFingerRotationFromAmplitude(HandSides.Right);
            
            SendCollisionData();
        }

        private void OnApplicationQuit()
        {
            byte[] haptics = { 0, 0, 0, 0, 0, 0 };
            _sender.SendHaptics(haptics);
            
            // Seems like muscle settings are saved, so reset
            for (int i = 0; i < DataLength.FingerRot; i++)
            {
                flexManager.SetBoneRotation((FingerRotationAmplitude_e)i, 0, HandSides.Right);
                flexManager.SetBoneRotation((FingerRotationAmplitude_e)i, 0, HandSides.Left);
            }

            humanPoseHandler?.Dispose();
            // namedPipeCommunicator.StopListening();
        }

        private float lastUpdateTime = 0;
        private float updateInterval = 0.03f;
        /// <summary>
        /// Send Collision Data To GUI
        /// </summary>
        private void SendCollisionData(bool[] collidersFlag = null)
        {
            if(Time.time - lastUpdateTime < updateInterval)
                return;
            
            lastUpdateTime = Time.time;
            byte[] data = new byte[] { };
            if (collidersFlag != null)
            {
                fingerManager.ConvertCollisionDataToBytes(ref data, collidersFlag);
            }
            else
            {
                fingerManager.ConvertCollisionDataToBytes(ref data);
            }

            hapticsManager.SetSMAHapticsInBytes(data);

            byte[] hapticsData = hapticsManager.GetHapticsBytes();
            DebugTool.ShowArrayLog(hapticsData, "HapticsData");
            Assert.AreEqual(hapticsData.Length, DataLength.HapticsDataBytes);

            // byte[] vibrationData = hapticsManager.GetVibrationBytes();
            // DebugTool.ShowArrayLog(vibrationData, "VibrationData");
            // Assert.AreEqual(vibrationData.Length, DataLength.VibrationLength);

            _sender.SendHaptics(hapticsData);
        }
        
        /// <summary>
        /// Set finger haptics condition in a certain section.
        /// You must call it with enable=false once after calling with enable=true.
        /// </summary>
        /// <param name="hand">Left or Right</param>
        /// <param name="section">section</param>
        /// <param name="enable">true or false</param>
        public void SetHaptics(HandSides hand, ColliderFinger_e section, bool enable)
        {
            SetHaptics(hand, section, HapticModules.bottomLeft, enable);
            SetHaptics(hand, section, HapticModules.topLeft, enable);
            SetHaptics(hand, section, HapticModules.bottomRight, enable);
            SetHaptics(hand, section, HapticModules.topRight, enable);
        }
        
        public void SetHaptics(HandSides hand, ColliderFinger_e section, HapticModules module, bool enable)
        {
            if(enable)
                hapticsManager.SetHapticsOn(hand, section, module);
            else
                hapticsManager.SetHapticsOff(hand, section, module);
        }
        
        /// <summary>
        /// Set Vibration Style in Glove Body.
        /// </summary>
        /// <param name="hand">Left or Right</param>
        /// <param name="amplitude">0.0 ~ 1.0</param>
        /// <param name="frequency">frequency[Hz]</param>
        /// <param name="duration">duration[s]</param>
        public void SetVibration(HandSides hand, float amplitude, float frequency, float duration)
        {
            _sender.SendVibration(hand, amplitude, frequency, duration);
        }

        public void SetFingerHapticsStrength(HandSides hand, int strength)
        {
            hapticsManager.SetFingerHapticsStrength(hand, strength);
        }

        public bool GetHaptics(HandSides hand, ColliderFinger_e section, HapticModules collider)
        {
            return hapticsManager.GetHaptics(hand, section, collider);
        }

        public VibrationType GetVibration(HandSides hand)
        {
            return hapticsManager.GetVibration(hand);
        }

        public int GetFingerHapticsStrength(HandSides hand)
        {
            return hapticsManager.GetFingerHapticsStrength(hand);
        }

        public Transform GetBoneTransform(HumanBodyBones bone)
        {
            return animator.GetBoneTransform(bone);
        }

        public void GetHumanPose(ref HumanPose humanPose)
        {
            humanPoseHandler.GetHumanPose(ref humanPose);
        }

        public void SetHumanPose(ref HumanPose humanPose)
        {
            humanPoseHandler.SetHumanPose(ref humanPose);
        }

        public Transform GetFingerTransform(HandSides hand, FingerRotationAmplitude_e bone)
        {
            // Only 15 flex bones have explicit transforms.
            if ((int)bone < 0 || (int)bone >= DataLength.FingerBone)
                return null;

            if (hand == HandSides.Right)
                return fingerTransformsRight[(int)bone];
            else
                return fingerTransformsLeft[(int)bone];
        }

        public void SetFingerRotation(HandSides hand, FingerRotationAmplitude_e bone, Quaternion rot)
        {
            if ((int)bone < 0 || (int)bone >= DataLength.FingerBone)
                return;

            if (hand == HandSides.Right)
                fingerTransformsRight[(int)bone].localRotation = rot;
            else
                fingerTransformsLeft[(int)bone].localRotation = rot;
        }

        public float GetFingerRotationAmplitude(HandSides hand, FingerRotationAmplitude_e index)
        {
            return fingerRotationAmplitude[(int)hand, (int)index];
        }

        public void SetFingerRotationAmplitude(HandSides hand, FingerRotationAmplitude_e index, float amplitude)
        {
            fingerRotationAmplitude[(int)hand, (int)index] = amplitude;
        }

        public ButtonState GetControllerInput(HandSides hand, ControllerButtonType type)
        {
            return _controllerInput.GetControllerInput(hand, type);
        }

        public float GetControllerInput(HandSides hand, ControllerFloatInputType type)
        {
            return _controllerInput.GetControllerInput(hand, type);
        }
    
        public void AddOnControllerInputHandler(HandSides hand, ControllerButtonType type, ControllerInputHandler handler)
        {
            _controllerInput.AddOnControllerInputHandler(hand, type, handler);
        }

        public void AddOffControllerInputHandler(HandSides hand, ControllerButtonType type, ControllerInputHandler handler)
        {
            _controllerInput.AddOffControllerInputHandler(hand, type, handler);
        }

        public void OnFlexDataReceived(FlexData data)
        {
            for (int i = 0; i < DataLength.FingerRot; i++)
            {
                var section = (FingerRotationAmplitude_e)i;
                SetFingerRotationAmplitude(data.Hand, section, data[section]);
            }
        }

        public void OnControllerDataReceived(ControllerData data)
        {
            _controllerInput.StoreControllerInput(data);
        }
        
        public void OnWristPoseReceived(WristPose pose)
        {
            if (pose.HandSide == HandSides.Left)
            {
                _leftWristPose = pose;
            }
            else
            {
                _rightWristPose = pose;
            }
        }

        public Quaternion GetWristPose(HandSides hand)
        {
            return hand == HandSides.Left ? _leftWristPose.Rotation : _rightWristPose.Rotation;
        }
    }
}