using UnityEngine;
public class NetworkConfigData
{
    public int MaxClients { get; }

    public NetworkConfigData(int maxClients)
    {
        MaxClients = maxClients;
    }
}