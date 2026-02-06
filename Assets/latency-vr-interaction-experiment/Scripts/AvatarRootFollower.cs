using UnityEngine;

public class AvatarRootFollower : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;

    void LateUpdate()
    {
        // 体の位置を頭の真下に移動（高さは床に固定）
        Vector3 newPos = _cameraTarget.position;
        newPos.y = transform.parent.position.y; // XR Originの床の高さ
        transform.position = newPos;

        // 体の向きを頭の回転に合わせる（水平方向のみ）
        Vector3 forward = _cameraTarget.forward;
        forward.y = 0;
        if (forward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(forward);
        }
    }
}