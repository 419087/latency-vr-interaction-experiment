using UnityEngine;
using Cysharp.Threading.Tasks;

public interface ITaskExcuter
{
    UniTask ExecuteTask(HandSide handSide);
    void CompleteCurrentTask(HandSide handSide1, HandSide handSide2);
}
