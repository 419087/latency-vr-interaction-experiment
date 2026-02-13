using UnityEngine;
using Unity.Netcode;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private ConnectionManager _connectionManagerPrefab;

    [Header("Network Settings")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private int _maxClients = 2;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_networkManager);

        var config = new NetworkConfigData(_playerPrefab, _maxClients);
        builder.RegisterInstance(config);

        builder.Register<PlayerSpawner>(Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_connectionManagerPrefab, Lifetime.Singleton);

        builder.RegisterEntryPoint<NetworkBootstrapper>();
    }
}