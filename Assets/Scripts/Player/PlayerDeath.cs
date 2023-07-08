using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private NavMeshAgent nva;
    private Animator animator;
    private bool isDying = false;
    private float deathTimer = 0f;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject joystick;

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDying) // Ölüm koşulu true olursa
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

            nva.enabled = false; //navmesh kapatılarak karakterin düşüşü gerçekleştir

            gameObject.tag = "Untagged"; // tag kapat (ölü player takip etmesi kapatıldı)

            sphereCollider.enabled = false; // Tekrar vurmayı engellemek için sphere kapatıldı
            isDying = true; // ölüm koşulu true yapıldı

            playerMovement.enabled = false; // Player hareketi öldükten sonra engellendi
            joystick.SetActive(false); // Joystick görüntüsünün gözükmesi engellendi

            deathTimer = 2f; // Ölüm süresi, Update içerisinde geri sayım 
        }
    }
}
