using UnityEngine;

public class FingerRotator : MonoBehaviour
{
    [Header("Bones (Proximal, Intermediate, Distal)")]
    public Transform[] joints = new Transform[3];

    [Header("Rotation Settings")]
    public Vector3 rotationAxis = new Vector3(0, 0, 1); // アバターによって異なります
    public float maxRotationAngle = 80f;               // 完全に握った時の角度

    private Quaternion[] initialRotations;

    void Start()
    {
        // 初期状態（指が伸びている状態）の回転を保存
        initialRotations = new Quaternion[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            if (joints[i] != null)
                initialRotations[i] = joints[i].localRotation;
        }
    }

    // Animatorの後に実行される必要があるので LateUpdate を使用
    public void UpdateFinger(float[] curlValues)
    {
        for (int i = 0; i < joints.Length; i++)
        {
            if (joints[i] == null) continue;

            // 初期回転に、現在の曲げ量を加算した回転を適用
            // クォータニオンの掛け算は「回転の追加」を意味します
            Quaternion targetRotation = Quaternion.Euler(rotationAxis * (curlValues[i] * maxRotationAngle));
            joints[i].localRotation = initialRotations[i] * targetRotation;
        }
    }
}