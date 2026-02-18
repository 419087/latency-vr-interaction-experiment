using UnityEngine;
using VContainer;

public class ServerCanvas : MonoBehaviour
{
    [SerializeField] private TaskStartButton _taskStartButton;
    public TaskStartButton TaskStartButton => _taskStartButton;

    private GameManager _gameManager;

    [Inject]
    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}
