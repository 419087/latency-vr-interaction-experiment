using UnityEngine;
using VContainer;
using TMPro;

public class ServerCanvas : MonoBehaviour
{
    [SerializeField] private TaskStartButton _taskStartButton;
    public TaskStartButton TaskStartButton => _taskStartButton;

    [SerializeField] private TMP_Text _client1Text;
    public TMP_Text Client1Text => _client1Text;

    [SerializeField] private TMP_Text _client2Text;
    public TMP_Text Client2Text => _client2Text;

    private GameManager _gameManager;

    [Inject]
    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}
