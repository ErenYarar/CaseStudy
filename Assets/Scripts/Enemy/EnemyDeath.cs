using UnityEngine;
using UnityEngine.AI;

public class EnemyDeath : MonoBehaviour
{
    private NavMeshAgent nva;
    private Animator animator;
    [SerializeField] SphereCollider sphereCollider;
    private bool isDead = false;
    private float deathTimer = 0f;

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) // Ölüm koşulu true olursa
        {
            deathTimer -= Time.deltaTime; // deathTimer süresini geriye doğru say
            if (deathTimer <= 0) // Eğer deathTimer 0 veya aşağıya inerse
            {
                gameObject.SetActive(false); // Karakteri verilen süre içinde kapat
            }
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Death"); // Ölüm animasyonu oynat
        nva.enabled = false; //navmesh kapatılarak karakterin düşüşü gerçekleştir
        gameObject.tag = "Untagged"; // tag kapat (ölü düşmanı takip etmesi kapatıldı)
        sphereCollider.enabled = false; // Tekrar vurmayı engellemek için sphere kapatıldı
        deathTimer = 2f; // Ölüm süresi, Update içerisinde geri sayım 
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border")) //"Border" tag'li duvara çarpma
        {
            Die(); // Die fonk. çalıştır
        }
    }
}
