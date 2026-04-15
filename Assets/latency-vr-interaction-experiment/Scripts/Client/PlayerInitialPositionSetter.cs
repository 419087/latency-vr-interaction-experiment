using UnityEngine;
using VContainer;
using System.Collections.Generic;
using VIVE.OpenXR;

public class PlayerInitialPositionSetter : MonoBehaviour
{
    private EntryConfig _entryConfig;
    private Dictionary<int, Vector3> _initialPosition = new Dictionary<int, Vector3>{ 
        { 0, new Vector3(0.1f, 0, 0) },
        { 1, new Vector3(-0.1f, 0, 0) } 
    };

    [Inject]
    public void Construct(EntryConfig entryConfig)
    {
        _entryConfig = entryConfig;
    }

    private void Start()
    {
        if (_entryConfig.IsServer) return;
        
        transform.position = _initialPosition[_entryConfig.PlayerId];
        transform.LookAt(Vector3.zero);
    }
}
