using UnityEngine;
using Unity.Netcode;
using System.Linq;
using VContainer;
using VContainer.Unity;
using UnityEngine.XR.Management;


#if UNITY_EDITOR
using Unity.Multiplayer.Playmode;
#endif

public class NetworkBootstrapper : IStartable
{
    private readonly IObjectResolver _resolver;
    private readonly NetworkManager _networkManagerPrefab;
    private readonly ConnectionManager _connectionManagerPrefab;
    private readonly GameManager _gameManagerPrefab;
    private readonly ServerCanvas _serverCanvasPrefab;

    public NetworkBootstrapper(
        IObjectResolver resolver,
        NetworkManager networkManager,
        ConnectionManager connectionManagerPrefab,
        GameManager gameManagerPrefab,
        ServerCanvas serverCanvasPrefab)
    {
        _resolver = resolver;
        _networkManagerPrefab = networkManager;
        _connectionManagerPrefab = connectionManagerPrefab;
        _gameManagerPrefab = gameManagerPrefab;
        _serverCanvasPrefab = serverCanvasPrefab;
    }

    public void Start()
    {
#if UNITY_EDITOR
        var tags = CurrentPlayer.ReadOnlyTags();

        if (tags.Contains("Server"))
        {
            InitializeServer();
        }
        else if (tags.Contains("Client"))
        {
            InitializeClientGame();
        }
        else
        {
            Debug.Log("ネットワークを使わずに起動します");
        }
#elif UNITY_SERVER
        InitializeDedicatedServer();
#else
        InitializeClientGame();
#endif
    }

    private void InitializeServer()
    {
        Debug.Log("VContainer [Server]: サーバーとして起動します");
        
        var connectionManagerInstance = _resolver.Instantiate(_connectionManagerPrefab);
        var gameManagerInstance = _resolver.Instantiate(_gameManagerPrefab);
        ServerCanvas serverCanvasInstance = _resolver.Instantiate(_serverCanvasPrefab);

        var taskStartButton = serverCanvasInstance.TaskStartButton;
        taskStartButton.Initialize(gameManagerInstance.GetComponent<GameManager>());

        connectionManagerInstance.InitializeConnectionManager();

        Application.targetFrameRate = 30;
        StopXR();

        _networkManagerPrefab.StartServer();
    }

    private void InitializeClientGame()
    {
        Debug.Log("VContainer [Client]: クライアントとして起動します");
        Application.targetFrameRate = 60;
        _networkManagerPrefab.StartClient();
    }

    private void StopXR()
    {
        var xrManager = XRGeneralSettings.Instance.Manager;
        if (xrManager != null && xrManager.isInitializationComplete)
        {
            Debug.Log("XR Subsystems を停止し、Loader を破棄します...");
            xrManager.StopSubsystems();
            xrManager.DeinitializeLoader();
        }
    }
}