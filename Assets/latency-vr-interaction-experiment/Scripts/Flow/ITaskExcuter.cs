using UnityEngine;
using Cysharp.Threading.Tasks;

public interface ITaskExcuter
{
    UniTask ExecuteTask(HandSide handSide);
    void TouchedHand(HandSide handSide1, HandSide handSide2);
}
