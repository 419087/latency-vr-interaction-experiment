using Unity.Netcode;
using UnityEngine;
using VContainer;

public class NetworkTaskMediator : NetworkBehaviour
{
    private ClientCanvas _clientCanvas;

    public void InitializeAsClient(ClientCanvas clientCanvas)
    {
        _clientCanvas = clientCanvas;
    }

    [ClientRpc]
    public void ShowHandSideTextClientRpc(HandSide handSide, ClientRpcParams rpcParams = default) => _clientCanvas.ShowHandSideText(handSide);

    [ClientRpc]
    public void ShowMessageTextClientRpc(string message) => _clientCanvas.ShowMessageText(message);

    [ClientRpc]
    public void HideAllTextClientRpc() => _clientCanvas.HideAllText();
}
