using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner
{
    private readonly GameObject _playerPrefab;
    public PlayerSpawner(NetworkConfigData config)
    {
        _playerPrefab = config.PlayerPrefab.gameObject;
    }

    public void SpawnPlayerForClient(ulong clientId)
    {
        Vector3 spawnPos = new Vector3(clientId * 2.0f, 0, 0);
        GameObject playerInstance = Object.Instantiate(_playerPrefab, spawnPos, Quaternion.identity);

        NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();

        // スポーンを実行して、接続したクライアントに所有権を付与
        netObj.SpawnWithOwnership(clientId);
    }
}
