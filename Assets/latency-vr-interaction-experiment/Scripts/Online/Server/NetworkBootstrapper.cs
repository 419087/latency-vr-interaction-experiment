using UnityEngine;
using Unity.Netcode;

public class NetworkBootstrapper : MonoBehaviour
{
    void Start()
    {
        // 1. ネットワーク起動前の、ビルドターゲットによる分岐
#if UNITY_SERVER
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
