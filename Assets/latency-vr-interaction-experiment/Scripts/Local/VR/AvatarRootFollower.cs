using UnityEngine;
using Unity.Netcode;

public class AvatarRootFollower : NetworkBehaviour
{
    private Transform _target;

    public void Construct(Transform target)
    {
        _target = target;
    }

    private void Start()
    {
        Debug.Log((this.transform.position.x, IsOwner));
    }

    void LateUpdate()
    {
        if (_target == null) return;
        if (!IsOwner) return;

        // 体の位置を頭の真下に移動（高さは床に固定）
        Vector3 newPos = _target.position;
        newPos.y = transform.parent.position.y; // XR Originの床の高さ
        transform.position = newPos;

        // 体の向きを頭の回転に合わせる（水平方向のみ）
        Vector3 forward = _target.forward;
        forward.y = 0;
        if (forward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(forward);
        }
    }
}