using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);
    [SerializeField] private float followSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}