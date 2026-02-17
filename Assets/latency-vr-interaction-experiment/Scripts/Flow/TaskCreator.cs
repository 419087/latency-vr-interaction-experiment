using UnityEngine;
using System.Collections.Generic;

public class TaskCreator
{
    private readonly int _taskIterations;

    public TaskCreator(int taskIterations)
    {
        _taskIterations = taskIterations;
    }

    /// <summary>
    /// Right, Left, Both を各10回ずつ、ランダムな順序で並べたListを返す
    /// </summary>
    public List<HandSide> CreateTaskList()
    {
        List<HandSide> taskList = new List<HandSide>();

        // 各enumを10回ずつ追加
        for (int i = 0; i < 10; i++)
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