using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ClientCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _leftPanel;
    [SerializeField] private GameObject _rightPanel;
    [SerializeField] private GameObject _bothPanel;
    [SerializeField] private GameObject _messagePanel;
    [SerializeField] private TextMeshProUGUI _messageText;

    [SerializeField] private int _textDisplayDurationSeconds = 3;

    private RectTransform _panelRect;
    private RectTransform _textRect;

    // 1文字あたりの幅（フォントサイズに合わせて調整）
    private const float widthPerChar = 100f; 
    private const float padding = 50f;

    private void Start()
    {
        _panelRect = _messagePanel.GetComponent<RectTransform>();
        _textRect = _messageText.GetComponent<RectTransform>();
    }

    public void ShowHandSideText(HandSide handSide)
    {
        _leftPanel.SetActive(handSide == HandSide.Left);
        _rightPanel.SetActive(handSide == HandSide.Right);
        _bothPanel.SetActive(handSide == HandSide.Both);
    }

    public async void ShowMessageText(string message)
    {
        _messagePanel.SetActive(true);

        // メッセージの長さに応じてパネルとテキストの幅を調整する
        float newWidth = message.Length * widthPerChar;
        _panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth + padding);
        _textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

        _messageText.text = message;

        await UniTask.Delay(_textDisplayDurationSeconds * 1000);
        HideAllText();
    }

    public void HideAllText()
    {
        _leftPanel.SetActive(false);
        _rightPanel.SetActive(false);
        _bothPanel.SetActive(false);
        _messagePanel.SetActive(false);
    }
}
