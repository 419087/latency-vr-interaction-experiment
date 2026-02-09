using Unity.Netcode;
using UnityEngine;

// サーバー側での接続管理とプレイヤースポーンを担当するクラス
public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private int _maxClients = 2;
    [SerializeField] private GameObject _playerPrefab;

    private PlayerSpawner _playerSpawner;

    private void Start()
    {
        _playerSpawner = new PlayerSpawner(_playerPrefab);

        if (NetworkManager.Singleton != null)
        {
            // 接続承認のコールバック
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            // 承認が完了し、接続が確立された後にスポーンさせるためのコールバック
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    // 接続数がmaxClients未満の場合のみ承認する(サーバーを除く)
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        int currentClientCount = 0;
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.ClientId != NetworkManager.ServerClientId)
            {
                currentClientCount++;
            }
        }

        if (currentClientCount < _maxClients)
        {
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
}