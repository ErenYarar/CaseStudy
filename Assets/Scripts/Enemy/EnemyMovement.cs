using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent nva; // Düşmanın NavMeshAgent bileşeni
    private Rigidbody enemyRb; // Düşmanın Rigidbody bileşeni
    private GameObject[] targets; // Hedefleri tutan dizi (diğer düşmanlar ve oyuncu)
    private GameObject player; // Oyuncu
    private Animator animator; // Düşmanın Animator bileşeni

    // Düşmana vurma gücü
    private float minFloat = 500f; // Etrafındaki nesnelere uygulanacak minimum vurma gücü
    private float maxFloat = 1000f; // Etrafındaki nesnelere uygulanacak maksimum vurma gücü
    private float forceHit; // Etrafındaki nesnelere uygulanacak vurma gücü

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>();  // Düşmanın NavMeshAgent bileşenini inspector'dan alır
        animator = GetComponent<Animator>(); // Düşmanın Animator bileşenini inspector'dan alır
        enemyRb = GetComponent<Rigidbody>();

        // Oyuncu ve diğer düşmanlar
        targets = GameObject.FindGameObjectsWithTag("Enemy"); // "Enemy" tag'ine sahip tüm düşmanları alır
        player = GameObject.FindGameObjectWithTag("Player"); // "Player" tag'ine sahip oyuncuyu alır
        targets = targets.Concat(new[] { player }).ToArray(); // Diziye oyuncuyu da ekler
    }

    private void FixedUpdate()
    {
        GameObject closestTarget = FindClosestTarget(); // En yakın hedefi bulur
        if (closestTarget != null) // Eğer en yakında target var ise
        {
            if (nva.isActiveAndEnabled)
            {
                // En yakın hedefe doğru hareket et
                nva.destination = closestTarget.transform.position;
            }
            else
            {
                Debug.LogWarning("NavMeshAgent is disabled."); 
            }
        }
        else
        {
            // Son düşman kaldıysa, doğrudan oyuncuyu takip et
            if (RandomSpawner.Instance.spawnCount == 1)
            {
                if (player != null)
                {
                    if (nva.isActiveAndEnabled)
                    {
                        nva.destination = player.transform.position;
                    }
                    else
                    {
                        Debug.LogWarning("NavMeshAgent is disabled.");
                    }
                }
            }
        }
    }

    private GameObject FindClosestTarget()
    {
        GameObject closestTarget = null; // En yakın hedef başlangıçta null olarak atanır

        // Başlangıçta en yakın mesafe sonsuz olarak atanır
        // Düşmanın herhangi bir hedefe olan başlangıç mesafesini belirlemek için en büyük değer kullanılır
        // En yakın hedefi bulmak için başlangıçta herhangi bir sınırlama olmamasını sağlar ve düşmanın herhangi bir hedefe daha yakın bir konuma hareket edebilmesini mümkün kılar.
        float closestDistance = Mathf.Infinity; 

        Vector3 currentPosition = transform.position; // Düşmanın mevcut pozisyonu

        foreach (GameObject target in targets)
        {
            if (target != null && target != gameObject)  // Kendi kendini hedef olarak almamak için kontrol yapılır
            {
                float distance = Vector3.Distance(target.transform.position, currentPosition); // Düşman ile hedef arasındaki mesafe hesaplanır
                if (distance < closestDistance) // Eğer bu hedef, önceki en yakın hedeften daha yakınsa
                {
                    closestTarget = target; // En yakın hedef olarak atanır
                    closestDistance = distance; // En yakın mesafe güncellenir
                }
            }
        }

        return closestTarget; // En yakın hedef döndürülür
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") 
        || collision.gameObject.CompareTag("Enemy")) // Etrafındaki diğer düşmanlara VEYA player'a çarptığında
        {
            animator.SetBool("isFloating", true); // "isFloating" animasyon parametresini true yapar

            forceHit = UnityEngine.Random.Range(minFloat, maxFloat); // Vurma gücünü rastgele belirler
            Vector3 moveDirection = enemyRb.transform.position - collision.transform.position;
            enemyRb.AddForce(moveDirection.normalized * forceHit); // Düşmana vurma gücünü uygular

            StartCoroutine(WaitForAnimEnded()); // Animasyonun bitmesini bekler
        }
    }

    IEnumerator WaitForAnimEnded()
    {
        yield return new WaitForSeconds(.5f); // 0.5 saniye bekletir sonra:
        animator.SetBool("isFloating", false); // "isFloating" animasyon parametresini false yapar
    }
}
