using UnityEngine;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;
using Unity.VisualScripting;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private ConnectionManager _connectionManagerPrefab;

    [Header("VR Rig Target Settings")]
    [SerializeField] private CameraMarker _cameraMarker;
    [SerializeField] private LeftControllerMarker _leftControllerMarker;
    [SerializeField] private RightControllerMarker _rightControllerMarker;


    [Header("Network Settings")]
    [SerializeField] private PlayerInitializer _playerPrefab;   // IPlayerPrefabMarkerを実装している必要がある(差し替え後に具象クラス型に変更)

    [SerializeField] private int _maxClients = 2;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_networkManager);

        var vrConfig = new VRConfigData(_cameraMarker, _leftControllerMarker, _rightControllerMarker);
        builder.RegisterInstance(vrConfig);

        var networkConfig = new NetworkConfigData(_playerPrefab, _maxClients);
        builder.RegisterInstance(networkConfig);

        builder.Register<PlayerSpawner>(Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_connectionManagerPrefab, Lifetime.Singleton);

        builder.RegisterEntryPoint<NetworkBootstrapper>();
    }
}