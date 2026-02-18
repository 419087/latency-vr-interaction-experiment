using Unity.Netcode;
using UnityEngine;
using VContainer;

// サーバー側での接続管理とプレイヤースポーンを担当するクラス
public class ConnectionManager : MonoBehaviour
{
    private int _maxClients;
    private NetworkManager _networkManager;
    private PlayerSpawner _playerSpawner;

    private int _currentClientCount = 0;

    [Inject]
    public void Construct(NetworkManager networkManager, NetworkConfigData config, PlayerSpawner playerSpawner)
    {
        _networkManager = networkManager;
        _maxClients = config.MaxClients;
        _playerSpawner = playerSpawner;
    }

    public void InitializeConnectionManager()
    {
        if (_networkManager != null)
        {
            // 接続承認のコールバック
            _networkManager.ConnectionApprovalCallback = ApprovalCheck;
            // 承認が完了し、接続が確立された後にスポーンさせるためのコールバック
            _networkManager.OnClientConnectedCallback += OnClientConnected;
            _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            _networkManager.ConnectionApprovalCallback = null;
            _networkManager.OnClientConnectedCallback -= OnClientConnected;
            _networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    // 接続数がmaxClients未満の場合のみ承認する(サーバーを除く)
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log($"接続可能か確認: 現在の接続数 {_currentClientCount} / 最大接続数 {_maxClients}");

        // サーバー自身の接続は無条件で通し、カウントもしない
        if (request.ClientNetworkId == NetworkManager.ServerClientId)
        {
            response.Approved = true;
            response.CreatePlayerObject = false;
            response.Pending = false;
            return;
        }

        if (_currentClientCount < _maxClients)
        {
            _currentClientCount++;

            response.Approved = true;
            // 手動でスポーンを制御するため、自動生成はオフにする
            response.CreatePlayerObject = false;
        }
        else
        {
            response.Approved = false;
            response.Reason = $"最大{_maxClients}人までしか接続できません。";
        }

        response.Pending = false;
    }

    private void OnClientConnected(ulong clientId)
    {
        // サーバー自身の接続時はスキップ
        if (clientId == NetworkManager.ServerClientId) return;

        Debug.Log($"Client {clientId} が承認・接続されました。アバターをスポーンします。");
        _playerSpawner.SpawnPlayerForClient(clientId);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        // サーバー自身の切断でなければカウントを減らす
        if (clientId != NetworkManager.ServerClientId)
        {
            _currentClientCount--;
            if (_currentClientCount < 0) _currentClientCount = 0;
            Debug.Log($"Client {clientId} が切断。現在のクライアント数: {_currentClientCount}");
        }
    }
}