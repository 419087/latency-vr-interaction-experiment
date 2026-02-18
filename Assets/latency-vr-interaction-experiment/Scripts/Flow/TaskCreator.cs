using UnityEngine;
using System.Collections.Generic;

public class TaskCreator
{
    private readonly int _taskIterations;

    public TaskCreator(TaskConfigData taskConfig)
    {
        _taskIterations = taskConfig.TaskIterations;
    }

    public List<HandSide> CreateTaskList()
    {
        List<HandSide> taskList = new List<HandSide>();

        // 各enumを_taskIterations回ずつ追加
        for (int i = 0; i < _taskIterations; i++)
        {
            taskList.Add(HandSide.Right);
            taskList.Add(HandSide.Left);
            taskList.Add(HandSide.Both);
        }

        // Fisher-Yatesシャッフル
        for (int i = taskList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (taskList[i], taskList[j]) = (taskList[j], taskList[i]);
        }

        return taskList;
    }
}