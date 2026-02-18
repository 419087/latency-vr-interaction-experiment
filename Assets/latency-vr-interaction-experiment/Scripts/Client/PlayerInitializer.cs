using UnityEngine;
using VContainer;
using Unity.Netcode;
using VContainer.Unity;


public class PlayerInitializer : NetworkBehaviour, IPlayerPrefabMarker
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
    public void Construct(VRConfigData vrConfig, ITaskExcuter taskExcuter)
    {
        _camerarRigFollower.Construct(vrConfig.CameraMarker.transform);
        _leftVrRigFollower.Construct(vrConfig.LeftControllerMarker.transform);
        _rightVrRigFollower.Construct(vrConfig.RightControllerMarker.transform);
        _avatarRootFollower.Construct(vrConfig.CameraMarker.transform);
        _fingerRotator.Construct(vrConfig.ContactGloveManager);
        if (IsServer)
        {
            _leftTouchInteractor.ConstructServer(vrConfig.LeftControllerMarker.HapticImpulsePlayer, taskExcuter);
            _rightTouchInteractor.ConstructServer(vrConfig.RightControllerMarker.HapticImpulsePlayer, taskExcuter);
        }
        else
        {
            _leftTouchInteractor.ConstructClient(vrConfig.LeftControllerMarker.HapticImpulsePlayer);
            _rightTouchInteractor.ConstructClient(vrConfig.RightControllerMarker.HapticImpulsePlayer);
        }
    }

    private void Start()
    {
        var scope = LifetimeScope.Find<GameLifetimeScope>();
        scope.Container.Inject(this);
    }
}
