using UnityEngine;

public class VRRigFollower : MonoBehaviour
{
    [SerializeField] private Transform _target; // 追従対象（XR Controllerなど）
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Vector3 _rotationOffset;

    void LateUpdate() // カメラの動きの後に実行
    {
        // 座標と回転を同期させる
        transform.position = _target.position + _target.TransformDirection(_positionOffset);
        transform.rotation = _target.rotation * Quaternion.Euler(_rotationOffset);
    }
}