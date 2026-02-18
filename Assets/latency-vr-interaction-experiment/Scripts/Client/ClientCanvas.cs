using UnityEngine;
using UnityEngine.UI;

public class ClientCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _leftText;
    [SerializeField] private GameObject _rightText;
    [SerializeField] private GameObject _bothText;

    public void ShowHandSideText(HandSide handSide)
    {
        _leftText.SetActive(handSide == HandSide.Left);
        _rightText.SetActive(handSide == HandSide.Right);
        _bothText.SetActive(handSide == HandSide.Both);
    }

    public void HideAllText()
    {
        _leftText.SetActive(false);
        _rightText.SetActive(false);
        _bothText.SetActive(false);
    }
}
