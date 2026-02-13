using UnityEngine;
public class NetworkConfigData
{
    public readonly GameObject PlayerPrefab;
    public readonly int MaxClients;

    public NetworkConfigData(GameObject playerPrefab, int maxClients)
    {
        PlayerPrefab = playerPrefab;
        MaxClients = maxClients;
    }
}