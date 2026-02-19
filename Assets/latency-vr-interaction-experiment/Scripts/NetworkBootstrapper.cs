using UnityEngine;
using Unity.Netcode;
using System.Linq;
using VContainer;
using VContainer.Unity;
using UnityEngine.XR.Management;
using System;
using Unity.Collections;

#if UNITY_EDITOR
using Unity.Multiplayer.Playmode;
#endif

public class NetworkBootstrapper : IStartable
{
    private readonly IObjectResolver _resolver;
    private readonly NetworkManager _networkManager;
    private readonly NetworkTaskMediator _networkTaskMediator;
    private readonly ConnectionManager _connectionManagerPrefab;
    private readonly GameManager _gameManagerPrefab;
    private readonly ServerCanvas _serverCanvasPrefab;
    private readonly ClientCanvas _clientCanvasPrefab;

    public NetworkBootstrapper(
        IObjectResolver resolver,
        NetworkManager networkManager,
        NetworkTaskMediator networkTaskMediator,
        ConnectionManager connectionManagerPrefab,
        GameManager gameManagerPrefab,
        ServerCanvas serverCanvasPrefab,
        ClientCanvas clientCanvasPrefab)
    {
        _resolver = resolver;
        _networkManager = networkManager;
        _networkTaskMediator = networkTaskMediator;
        _connectionManagerPrefab = connectionManagerPrefab;
        _gameManagerPrefab = gameManagerPrefab;
        _serverCanvasPrefab = serverCanvasPrefab;
        _clientCanvasPrefab = clientCanvasPrefab;
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

        _networkManager.StartServer();
    }

    private void InitializeClientGame(int playerId = 0, int participantId = 0)
    {
        Debug.Log("VContainer [Client]: クライアントとして起動します");

        Application.targetFrameRate = 60;

        var clientCanvasInstance = _resolver.Instantiate(_clientCanvasPrefab);
        _networkTaskMediator.InitializeAsClient(clientCanvasInstance);

        // プレイヤーIDと参加者IDを接続データとして送る
        using (var writer = new FastBufferWriter(sizeof(int) * 2, Allocator.Temp))
        {
            writer.WriteValueSafe(playerId);
            writer.WriteValueSafe(participantId);

            _networkManager.NetworkConfig.ConnectionData = writer.ToArray();
        }

        _networkManager.StartClient();
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