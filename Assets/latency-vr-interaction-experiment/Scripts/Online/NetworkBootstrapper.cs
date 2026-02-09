using UnityEngine;
using Unity.Netcode;
using System.Linq;

#if UNITY_EDITOR
using Unity.Multiplayer.Playmode;
#endif

public class NetworkBootstrapper : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        // MPPM経由で起動しているか確認
        var tags = CurrentPlayer.ReadOnlyTags();

        if (tags.Contains("Server"))
        {
            Debug.Log("MPPM [Server]: サーバーとして起動します");
            InitializeDedicatedServer();
        }
        else if (tags.Contains("Client"))
        {
            Debug.Log("MPPM [Client]: クライアントとして起動します");
            InitializeClientGame();
        }
        else
        {
            // MPPMを使っていない、またはServer/Clientのタグがないときの挙動
            Debug.Log("ネットワークを使わずに起動します");
        }

#elif UNITY_SERVER
        InitializeDedicatedServer();
#else
        InitializeClientGame();
#endif
    }

    // サーバー用の初期化
    private void InitializeDedicatedServer()
    {
        // サーバー固有の設定（フレームレート制限など）
        Application.targetFrameRate = 30;

        // サーバーとして起動
        NetworkManager.Singleton.StartServer();
    }

    // クライアント用の初期化
    private void InitializeClientGame()
    {
        // クライアント固有の設定
        Application.targetFrameRate = 60;

        // クライアントとして接続
        NetworkManager.Singleton.StartClient();
    }
}
