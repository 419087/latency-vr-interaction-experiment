using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerButton : MonoBehaviour
{
    [SerializeField] private EntryConfig _entryConfig;

    public void OnServerButtonClicked()
    {
        Debug.Log($"サーバーとして起動します");
        _entryConfig.SetEntryConfigAsServer();
        SceneManager.LoadScene("ExperimentScene");
    }
}
