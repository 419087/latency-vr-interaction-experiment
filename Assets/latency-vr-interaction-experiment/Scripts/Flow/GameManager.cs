using System;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    private TaskCreator _taskCreator;
    private ITaskExcuter _taskExcuter;
    private NetworkTaskMediator _networkTaskMediator;

    private (int min, int max) _taskIntervalRange;

    [Inject]
    public void Construct(TaskCreator taskCreator, ITaskExcuter taskExcuter, NetworkTaskMediator networkTaskMediator, TaskConfigData taskConfig)
    {
        _taskCreator = taskCreator;
        _taskExcuter = taskExcuter;
        _networkTaskMediator = networkTaskMediator;
        _taskIntervalRange = (taskConfig.MinIntervalMilliSeconds, taskConfig.MaxIntervalMilliSeconds);
    }

    public async void StartGame()
    {
        Debug.Log("タスクを開始します");
        
        _networkTaskMediator.ShowMessageTextClientRpc("タスクを開始します");

        var tasks = _taskCreator.CreateTaskList();

        for (int i = 0; i < tasks.Count; i++)
        {

            int randomMilliseconds = UnityEngine.Random.Range(_taskIntervalRange.min, _taskIntervalRange.max);
            await UniTask.Delay(TimeSpan.FromMilliseconds(randomMilliseconds));

            await _taskExcuter.ExecuteTask(tasks[i]);

            _networkTaskMediator.ShowMessageTextClientRpc("OK");
        }

        Debug.Log("すべてのタスクが完了しました");
        _networkTaskMediator.ShowMessageTextClientRpc("すべてのタスクが完了しました");
    }
}
