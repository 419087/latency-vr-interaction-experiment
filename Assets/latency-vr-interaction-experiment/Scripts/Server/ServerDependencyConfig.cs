using UnityEngine;

public class ServerDependencyConfig
{
    public ConnectionManager ConnectionManagerPrefab { get; }
    public GameManager GameManagerPrefab { get; }
    public GameObject ServerCanvasPrefab { get; }

    public ServerDependencyConfig(ConnectionManager connectionManagerPrefab, GameManager gameManagerPrefab, GameObject serverCanvasPrefab)
    {
        ConnectionManagerPrefab = connectionManagerPrefab;
        GameManagerPrefab = gameManagerPrefab;
        ServerCanvasPrefab = serverCanvasPrefab;
    }
}
