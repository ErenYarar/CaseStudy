using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private NavMeshAgent nva; // Oyuncunun NavMeshAgent bileşeni
    private Animator animator; // Oyuncunun Animator bileşeni
    private bool isDying = false; // Ölüm durumunu belirten boolean değeri
    private float deathTimer = 0f; // Ölüm sonrası bekleme süresi
    [SerializeField] SphereCollider sphereCollider; // Oyuncunun SphereCollider bileşeni
    [SerializeField] PlayerMovement playerMovement; // Oyuncunun PlayerMovement bileşeni
    [SerializeField] GameObject joystick; // Joystick oyun nesnesi

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>(); // Oyuncunun NavMeshAgent bileşenini inspector'dan alır
        animator = GetComponent<Animator>(); // Oyuncunun Animator bileşenini inspector'dan alır
    }

    private void Update()
    {
        if (isDying) // Oyuncu öldüyse
        {
            deathTimer -= Time.deltaTime; // deathTimer süresini geriye doğru say
            if (deathTimer <= 0) // Eğer deathTimer 0 veya aşağıya inerse
            {
                SceneManager.LoadScene("Game"); // Sahneyi yeniden başlat
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border") && !isDying) // "Border" tag'li duvara çarpma VE ölüm koşulu 
        {
            animator.SetTrigger("Death"); // Ölüm animasyonu oynat

            nva.enabled = false; //navmesh kapatılarak karakterin düşüşü gerçekleştirir

            gameObject.tag = "Untagged"; // tag kapat (ölü player takip etmesi kapatıldı)

            sphereCollider.enabled = false; // Tekrar vurmayı engellemek için sphere kapatıldı

            isDying = true; // ölüm koşulu true yapar

            playerMovement.enabled = false; // Player hareketi öldükten sonra engellendi

            joystick.SetActive(false); // Joystick görüntüsünün gözükmesi engellendi

            deathTimer = 2f; // Ölüm süresi, Update içerisinde geri sayım 
        }
    }
}
