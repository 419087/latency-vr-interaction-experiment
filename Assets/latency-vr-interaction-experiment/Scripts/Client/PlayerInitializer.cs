using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerInitializer : MonoBehaviour, IPlayerPrefabMarker
{
    
    [SerializeField] private VRRigFollower _camerarRigFollower;
    [SerializeField] private VRRigFollower _leftVrRigFollower;
    [SerializeField] private VRRigFollower _rightVrRigFollower;
    [SerializeField] private AvatarRootFollower _avatarRootFollower;
    [SerializeField] private FingerRotator _fingerRotator;
    [SerializeField] private TouchInteractor _leftTouchInteractor;
    [SerializeField] private TouchInteractor _rightTouchInteractor;

    public GameObject GameObject => this.gameObject;

    [Inject]
    public void Construct(VRConfigData vrConfig)
    {
        _camerarRigFollower.Construct(vrConfig.CameraMarker.transform);
        _leftVrRigFollower.Construct(vrConfig.LeftControllerMarker.transform);
        _rightVrRigFollower.Construct(vrConfig.RightControllerMarker.transform);
        _avatarRootFollower.Construct(vrConfig.CameraMarker.transform);
        _fingerRotator.Construct(vrConfig.ContactGloveManager);
        _leftTouchInteractor.Construct(vrConfig.LeftControllerMarker.HapticImpulsePlayer);
        _rightTouchInteractor.Construct(vrConfig.RightControllerMarker.HapticImpulsePlayer);
    }
}
