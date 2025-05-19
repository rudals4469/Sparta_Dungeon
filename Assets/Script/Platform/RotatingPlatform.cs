using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public Transform centerPoint;    // 회전 중심점
    public float radius = 2f;        // 회전 반지름
    public float rotationSpeed = 1f; // 초당 회전 속도 (도 단위가 아님, 라디안처럼 동작)

    private float _angle = 0f;

    void Update()
    {
        _angle += rotationSpeed * Time.deltaTime;

        float x = Mathf.Cos(_angle) * radius;
        float z = Mathf.Sin(_angle) * radius;

        transform.position = centerPoint.position + new Vector3(x, 0f, z);
    }
}