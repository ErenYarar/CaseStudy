using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] Transform player; // Karakter Transform
    [SerializeField] float smoothSpeed; // Kamera hareketinin yumuşaklığı
    private Vector3 _currentVelocity = Vector3.zero; // Kamera ile karakter arasındaki mesafe

    private void Awake()
    {
        _offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        Vector3 playerPosition = player.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref _currentVelocity, smoothSpeed);
    }
}
