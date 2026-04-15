using UnityEngine;
using Unity.Netcode;

public class VRRigFollower : NetworkBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Vector3 _rotationOffset;

    public void Construct(Transform target)
    {
        _target = target;
    }

    void LateUpdate() // カメラの動きの後に実行
    {
        if (_target == null) return;
        if (!IsOwner) return;

        // 座標と回転を同期させる
        transform.position = _target.position + _target.TransformDirection(_positionOffset);
        transform.rotation = _target.rotation * Quaternion.Euler(_rotationOffset);
    }
}