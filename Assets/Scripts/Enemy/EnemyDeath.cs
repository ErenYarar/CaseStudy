using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyDeath : MonoBehaviour
{
    private NavMeshAgent nva; // Düşmanın NavMeshAgent bileşeni
    private Animator animator; // Düşmanın Animator bileşeni
    [SerializeField] SphereCollider sphereCollider; // Düşmanın SphereCollider bileşeni
    private bool isDead = false; // Düşmanın ölü olup olmadığını belirten boolean değeri
    private float deathTimer = 0f; // Ölüm sonrası bekleme süresi
    [SerializeField] private bool isPlayerCollision = false; // Oyuncuyla çarpışmanın gerçekleşip gerçekleşmediğini belirten boolean değeri

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>(); // Düşmanın NavMeshAgent bileşenini inspector'dan alır
        animator = GetComponent<Animator>(); // Düşmanın Animator bileşenini inspector'dan alır
    }

    private void Update()
    {
        if (isDead)  // Düşman öldüyse
        {
            deathTimer -= Time.deltaTime; // deathTimer süresini geriye doğru sayar
            if (deathTimer <= 0) // Eğer deathTimer 0 veya aşağıya inerse
            {
                if (RandomSpawner.Instance.spawnCount <= 0) // Eğer üretilmesi gereken düşman sayısı 0 veya aşağısındaysa
                {
                    SceneManager.LoadScene("Game");  // oyunu yeniden başlat
                }
                else
                {
                    gameObject.SetActive(false); // Belirtilen süre içinde düşmanın aktifliğini kapatır
                }
            }
        }
    }

    private void Die()
    {
        RandomSpawner.Instance.UpdateEnemyCount(); // Üretilmesi gereken düşman sayısını azaltıp, Text yazdırır
        isDead = true; // Düşman öldü
        animator.SetTrigger("Death"); // Ölüm animasyonu oynatır
        nva.enabled = false; // Düşmanın NavMeshAgent bileşenini devre dışı bırakarak hareketini durdurur
        gameObject.tag = "Untagged"; // Düşmanın tag'ini kaldırır (ölü düşmanı takip etmeyi durdur)
        sphereCollider.enabled = false; // Tekrar vurulmayı engellemek için SphereCollider'ı devre dışı bırak
        deathTimer = 2f; // Ölüm sonrası bekleme süresi, Update içinde geriye doğru sayılacak
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border") && isPlayerCollision) // Eğer düşmanın son çarptığı Player ise VE "Border" tag'li duvara çarparsa
        {
            ScoreManager.Instance.IncreaseScore(); // Skor artışı gerçekleştirilir
            Die(); // Die fonk. çalıştır
        }
        else if (other.CompareTag("Border")) //"Border" tag'li duvara çarpma
        {
            Die(); // Die fonk. çalıştır
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) // Player ile çarpışma
        {
            isPlayerCollision = true; // Player ile çarpışma gerçekleşti
        }
        if (other.gameObject.CompareTag("Enemy")) // Enemy ile çarpışma
        {
            isPlayerCollision = false; // Enemy ile çarpışma gerçekleşti
        }
    }
}
