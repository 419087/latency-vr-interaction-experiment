using UnityEngine;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;

public class PlayerSpawner
{
    private IObjectResolver _resolver;
    private readonly PlayerInitializer _playerPrefab;
    public PlayerSpawner(IObjectResolver resolver, PlayerInitializer playerPrefab)
    {
        _resolver = resolver;
        _playerPrefab = playerPrefab;
    }

    public void SpawnPlayerForClient(ulong clientId)
    {
        Vector3 spawnPos = new Vector3(clientId * 2.0f, 0, 0);
        PlayerInitializer playerInstance = _resolver.Instantiate(_playerPrefab, spawnPos, Quaternion.identity);

        NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();

        // スポーンを実行して、接続したクライアントに所有権を付与
        netObj.SpawnWithOwnership(clientId);
    }
}
