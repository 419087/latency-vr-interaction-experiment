using System;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    private TaskCreator _taskCreator;
    private ITaskExcuter _taskExcuter;
    private NetworkTaskMediator _networkTaskMediator;
    private UpstreamLatencyMeasurer _upstreamLatencyMeasurer;
    private PlayerData _playerData;
    private ResultCounter _resultCounter;
    private ResultWriter _resultWriter;

    private (int min, int max) _taskIntervalRange;

    [Inject]
    public void Construct(TaskCreator taskCreator, ITaskExcuter taskExcuter, NetworkTaskMediator networkTaskMediator, UpstreamLatencyMeasurer upstreamLatencyMeasurer, PlayerData playerData, TaskConfigData taskConfig, ResultCounter resultCounter, ResultWriter resultWriter)
    {
        _taskCreator = taskCreator;
        _taskExcuter = taskExcuter;
        _networkTaskMediator = networkTaskMediator;
        _upstreamLatencyMeasurer = upstreamLatencyMeasurer;
        _playerData = playerData;
        _resultCounter = resultCounter;
        _resultWriter = resultWriter;
        _taskIntervalRange = (taskConfig.MinIntervalMilliSeconds, taskConfig.MaxIntervalMilliSeconds);
    }

    public async void StartGame(int latencyCondition)
    {
        Debug.Log($"遅延タスクを開始します(遅延条件: {latencyCondition}ms)");
        _networkTaskMediator.ShowMessageTextClientRpc("タスクを開始します");

        _upstreamLatencyMeasurer.StartMeasureUpstreamLatencyClientRpc();

        _playerData.SetLatencyCondition(latencyCondition);

        var tasks = _taskCreator.CreateTaskList();
        _resultCounter.SetTaskHand(tasks);

        for (int i = 0; i < tasks.Count; i++)
        {

            int randomMilliseconds = UnityEngine.Random.Range(_taskIntervalRange.min, _taskIntervalRange.max);
            await UniTask.Delay(TimeSpan.FromMilliseconds(randomMilliseconds));

            await _taskExcuter.ExecuteTask(tasks[i]);

            _networkTaskMediator.ShowMessageTextClientRpc("OK");
        }

        Debug.Log("すべてのタスクが完了しました");
        _networkTaskMediator.ShowMessageTextClientRpc("すべてのタスクが完了しました");

        _upstreamLatencyMeasurer.StopMeasureUpstreamLatencyClientRpc();

        var writableTaskResult = _resultCounter.GetWritableTaskResult();
        _resultWriter.WriteResultsToCSV(writableTaskResult, "TaskResult");

        _upstreamLatencyMeasurer.SaveLatencyClientRpc();
    }
}
