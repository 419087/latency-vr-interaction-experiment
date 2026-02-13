using Unity.Netcode;
using UnityEngine;

// サーバー側での接続管理とプレイヤースポーンを担当するクラス
public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private int _maxClients = 2;
    [SerializeField] private GameObject _playerPrefab;

    private int _currentClientCount = 0;

    private PlayerSpawner _playerSpawner;

    public void InitializeConnectionManager()
    {
        _playerSpawner = new PlayerSpawner(_playerPrefab);

        if (NetworkManager.Singleton != null)
        {
            // 接続承認のコールバック
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            // 承認が完了し、接続が確立された後にスポーンさせるためのコールバック
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = null;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
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