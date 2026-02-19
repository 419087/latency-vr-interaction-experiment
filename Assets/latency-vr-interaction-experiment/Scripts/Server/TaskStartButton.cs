using UnityEngine;
using TMPro;

public class TaskStartButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField _taskConfigInputField;
    private GameManager _gameManager;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnStartButtonClicked()
    {
        if (_gameManager != null)
        {
            int latencyCondition;

            if (int.TryParse(_taskConfigInputField.text, out latencyCondition))
            {
                _gameManager.StartGame(latencyCondition);
            }
            else
            {
                Debug.LogWarning("無効なタスク設定です");
            }
        }
        else
        {
            Debug.LogError("GameManager not found.");
        }
    }
}
