using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] Transform player; // Karakter Transform
    [SerializeField] float smoothSpeed; // Kamera hareketinin yumuşaklığı
    private Vector3 _currentVelocity = Vector3.zero; // Kamera ile karakter arasındaki mesafe

    private void Awake()
    {
        // Kamera ve karakter arasındaki başlangıç mesafesini hesaplar
        _offset = transform.position - player.position;
    }

    private void LateUpdate() // Diğer tüm güncelleme işlemlerinden sonra çalışır ve kameranın konumunu günceller
    {
        // Kameranın takip edeceği konumu hesaplar
        Vector3 playerPosition = player.position + _offset;

        // Kamerayı yumuşak bir şekilde hedef konuma doğru hareket ettirir
        // smoothSpeed değeriyle belirtilen hızda hareket eder
        // _currentVelocity değişkeni, kameranın hızını takip etmek için kullanılır
        // Vector3.SmoothDamp() fonksiyonu hedef konuma doğru yumuşak bir şekilde hareket ettirilmesi sağlanır
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref _currentVelocity, smoothSpeed);
    }
}
