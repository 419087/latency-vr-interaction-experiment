using System.Threading.Tasks;
using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    private TaskCreator _taskCreator;
    private ITaskExcuter _taskExcuter;

    [Inject]
    public void Construct(TaskCreator taskCreator, ITaskExcuter taskExcuter)
    {
        _taskCreator = taskCreator;
        _taskExcuter = taskExcuter;
    }

    public async void StartGame()
    {
        var tasks = _taskCreator.CreateTaskList();

        for (int i = 0; i < tasks.Count; i++)
        {
            await _taskExcuter.ExecuteTask(tasks[i]);
        }

        Debug.Log("All tasks completed!");
    }
}
