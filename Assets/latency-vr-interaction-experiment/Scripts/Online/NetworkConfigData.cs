using UnityEngine;
public class NetworkConfigData
{
    public PlayerInitializer PlayerPrefab { get; }
    public int MaxClients { get; }

    public NetworkConfigData(PlayerInitializer playerPrefab, int maxClients)
    {
        PlayerPrefab = playerPrefab;
        MaxClients = maxClients;
    }
}