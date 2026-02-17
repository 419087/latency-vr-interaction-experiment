using UnityEngine;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;
using ContactGloveSDK;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private ConnectionManager _connectionManagerPrefab;

    [Header("VR Target Settings")]
    [SerializeField] private CameraMarker _cameraMarker;
    [SerializeField] private LeftControllerMarker _leftControllerMarker;
    [SerializeField] private RightControllerMarker _rightControllerMarker;
    [SerializeField] private ContactGloveManager _contactGloveManager;


    [Header("Network Settings")]
    [SerializeField] private PlayerInitializer _playerPrefab;   // IPlayerPrefabMarkerを実装している必要がある(差し替え後に具象クラス型に変更)

    [SerializeField] private int _maxClients = 2;

    [Header("Task Settings")]
    [SerializeField] private int _taskIterations = 10;
    [SerializeField] private GameManager _gameManager;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_networkManager);

        var vrConfig = new VRConfigData(_cameraMarker, _leftControllerMarker, _rightControllerMarker, _contactGloveManager);
        var networkConfig = new NetworkConfigData(_playerPrefab, _maxClients);
        var taskCreator = new TaskCreator(_taskIterations);

        builder.RegisterInstance(vrConfig);
        builder.RegisterInstance(networkConfig);
        builder.RegisterInstance(taskCreator);
        builder.RegisterInstance<ITaskExcuter>(new TmpTaskExcuter());

        builder.RegisterInstance(_gameManager);

        builder.Register<PlayerSpawner>(Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_connectionManagerPrefab, Lifetime.Singleton);

        builder.RegisterEntryPoint<NetworkBootstrapper>();
    }
}