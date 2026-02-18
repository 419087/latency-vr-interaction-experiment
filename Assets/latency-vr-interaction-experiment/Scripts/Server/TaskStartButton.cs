using UnityEngine;
using VContainer;

public class TaskStartButton : MonoBehaviour
{
    private GameManager _gameManager;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnStartButtonClicked()
    {
        if (_gameManager != null)
        {
            _gameManager.StartGame();
        }
        else
        {
            Debug.LogError("GameManager not found.");
        }
    }
}
