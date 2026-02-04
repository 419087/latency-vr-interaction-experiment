using UnityEngine;

public class VRRigFollower : MonoBehaviour
{
    [SerializeField] private Transform target; // 追従対象（XR Controllerなど）
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;

    void LateUpdate() // カメラの動きの後に実行
    {
        // 座標と回転を同期させる
        transform.position = target.position + target.TransformDirection(positionOffset);
        transform.rotation = target.rotation * Quaternion.Euler(rotationOffset);
    }
}