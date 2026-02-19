using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] private int _playerId;
    [SerializeField] private EntryConfig _entryConfig;
    [SerializeField] private TMP_InputField  inputField;

    public void OnPlayerButtonClicked()
    {
        int playerId;

        if (int.TryParse(inputField.text, out playerId))
        {
            Debug.Log($"プレイヤー{_playerId}として参加します(参加者{inputField.text})");
            _entryConfig.SetEntryConfigAsClient(_playerId, playerId);
            SceneManager.LoadScene("ExperimentScene");
        }
        else
        {
            Debug.LogWarning("無効な参加者IDです");
        }
    }
}
