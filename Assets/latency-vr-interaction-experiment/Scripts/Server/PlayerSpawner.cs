using UnityEngine;
using Unity.Netcode;
using VContainer;

public class PlayerSpawner
{
    private readonly PlayerInitializer _playerPrefab;

    public PlayerSpawner(PlayerInitializer playerPrefab)
    {
        _playerPrefab = playerPrefab;
    }

    public void SpawnPlayerForClient(ulong clientId)
    {
        PlayerInitializer playerInstance = Object.Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);

        NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();

        // スポーンを実行して、接続したクライアントに所有権を付与
        netObj.SpawnWithOwnership(clientId);
    }
}
