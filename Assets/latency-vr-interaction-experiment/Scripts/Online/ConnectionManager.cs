using Unity.Netcode;
using UnityEngine;

public class ServerOnlyConnectionMonitor : MonoBehaviour
{
    [SerializeField] private int maxClients = 2; // クライアントの最大数

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // 現在接続中の「クライアント」の数を計算
        int currentClientCount = 0;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            // サーバー（通常 ID 0）以外のクライアントをカウント
            // Dedicated Serverとして動いている場合、サーバー自身はリストに含まれない設定もありますが、
            // 安全のために「サーバーでないこと」を確認します。
            if (client.ClientId != NetworkManager.ServerClientId)
            {
                currentClientCount++;
            }
        }

        // 判定
        if (currentClientCount < maxClients)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
        }
        else
        {
            response.Approved = false;
            response.Reason = $"このサーバーでは最大{maxClients}人のクライアントまでしか接続できません。";
            Debug.Log($"Rejected Client {request.ClientNetworkId}: Max clients reached.");
        }

        response.Pending = false;
    }
}