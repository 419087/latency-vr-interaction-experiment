using UnityEngine;
using Cysharp.Threading.Tasks;

public interface ITaskExcuter
{
    UniTask ExecuteTask(HandSide handSide);
}
