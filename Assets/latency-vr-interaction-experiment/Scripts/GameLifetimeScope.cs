using UnityEngine;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;
using ContactGloveSDK;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private EntryConfig _entryConfig;
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private NetworkTaskMediator _networkTaskMediator;
    [SerializeField] private ConnectionManager _connectionManagerPrefab;
    [SerializeField] private GameManager _gameManagerPrefab;
    [SerializeField] private ServerCanvas _serverCanvasPrefab;
    [SerializeField] private ClientCanvas _clientCanvasPrefab;

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
    [SerializeField] private int _minTaskIntervalMilliSeconds = 5000;
    [SerializeField] private int _maxTaskIntervalMilliSeconds = 10000;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_networkManager);
        builder.RegisterComponent(_networkTaskMediator);
        
        var vrConfig = new VRConfigData(
            _cameraMarker,
            _leftControllerMarker,
            _rightControllerMarker,
            _contactGloveManager);
        var networkConfig = new NetworkConfigData(_maxClients);
        var taskConfig = new TaskConfigData(_taskIterations, _minTaskIntervalMilliSeconds, _maxTaskIntervalMilliSeconds);

        builder.RegisterInstance(_entryConfig);
        builder.RegisterInstance(vrConfig);
        builder.RegisterInstance(networkConfig);
        builder.RegisterInstance(taskConfig);
        builder.RegisterInstance(_connectionManagerPrefab);
        builder.RegisterInstance(_gameManagerPrefab);
        builder.RegisterInstance(_serverCanvasPrefab);
        builder.RegisterInstance(_clientCanvasPrefab);
        builder.RegisterInstance(_playerPrefab);

        builder.Register<PlayerSpawner>(Lifetime.Singleton);
        builder.Register<TaskExcuter>(Lifetime.Singleton).As<ITaskExcuter>();;
        builder.Register<TaskCreator>(Lifetime.Singleton);
        builder.Register<PlayerData>(Lifetime.Singleton);
        builder.Register<ResultCounter>(Lifetime.Singleton);

        builder.RegisterEntryPoint<NetworkBootstrapper>();
    }
}