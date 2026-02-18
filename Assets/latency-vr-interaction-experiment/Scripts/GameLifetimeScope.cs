using UnityEngine;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;
using ContactGloveSDK;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private ConnectionManager _connectionManagerPrefab;
    [SerializeField] private GameManager _gameManagerPrefab;
    [SerializeField] private ServerCanvas _serverCanvasPrefab;

    [Header("VR Target Settings")]
    [SerializeField] private CameraMarker _cameraMarker;
    [SerializeField] private LeftControllerMarker _leftControllerMarker;
    [SerializeField] private RightControllerMarker _rightControllerMarker;
    [SerializeField] private ContactGloveManager _contactGloveManager;


    [Header("Network Settings")]
    [SerializeField] private PlayerInitializer _playerPrefab;

    [SerializeField] private int _maxClients = 2;

    [Header("Task Settings")]
    [SerializeField] private int _taskIterations = 10;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_networkManager);
        
        var vrConfig = new VRConfigData(
            _cameraMarker,
            _leftControllerMarker,
            _rightControllerMarker,
            _contactGloveManager);
        var networkConfig = new NetworkConfigData(_maxClients);
        var taskConfig = new TaskConfigData(_taskIterations);

        builder.RegisterInstance(vrConfig);
        builder.RegisterInstance(networkConfig);
        builder.RegisterInstance(taskConfig);
        builder.RegisterInstance(_connectionManagerPrefab);
        builder.RegisterInstance(_gameManagerPrefab);
        builder.RegisterInstance(_serverCanvasPrefab);
        builder.RegisterInstance(_playerPrefab);

        builder.Register<PlayerSpawner>(Lifetime.Singleton);
        builder.Register<TmpTaskExcuter>(Lifetime.Singleton);
        builder.Register<TaskCreator>(Lifetime.Singleton);

        builder.RegisterEntryPoint<NetworkBootstrapper>();
    }
}