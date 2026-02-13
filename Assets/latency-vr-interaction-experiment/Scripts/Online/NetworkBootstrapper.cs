using UnityEngine;
using Unity.Netcode;
using System.Linq;
using VContainer;
using VContainer.Unity;

#if UNITY_EDITOR
using Unity.Multiplayer.Playmode;
#endif

public class NetworkBootstrapper : IStartable
{

    private readonly NetworkManager _networkManager;
    private readonly ConnectionManager _connectionManager;

    [Inject]
    public NetworkBootstrapper(
        NetworkManager networkManager, 
        ConnectionManager connectionManager)
    {
        _networkManager = networkManager;
        _connectionManager = connectionManager;
    }

    public void Start()
    {
#if UNITY_EDITOR
        var tags = CurrentPlayer.ReadOnlyTags();

        if (tags.Contains("Server"))
        {
            InitializeDedicatedServer();
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

    private void InitializeDedicatedServer()
    {
        Debug.Log("VContainer [Server]: サーバーとして起動します");
        
        _connectionManager.InitializeConnectionManager();

        Application.targetFrameRate = 30;
        _networkManager.StartServer();
    }

    private void InitializeClientGame()
    {
        Debug.Log("VContainer [Client]: クライアントとして起動します");
        Application.targetFrameRate = 60;
        _networkManager.StartClient();
    }
}