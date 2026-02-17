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
        CheckScenes();
        StopXR();
        _networkManager.StartServer();
    }

    void CheckScenes()
    {
        Debug.Log($"ビルド設定に登録されているシーン:{UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings}");
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            Debug.Log($"Index {i}: {path}");
        }
    }

    private void InitializeClientGame()
    {
        Debug.Log("VContainer [Client]: クライアントとして起動します");
        Application.targetFrameRate = 60;
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