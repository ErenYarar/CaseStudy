using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent nva;
    private Rigidbody enemyRb;
    private GameObject[] targets;
    private Animator animator;

    // Düşmana vurma gücü
    private float minFloat = 800f;
    private float maxFloat = 1000f;
    private float forceHit;

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();

        // Hedefleri ayarla: Oyuncu ve diğer düşmanlar
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        targets = targets.Concat(new[] { GameObject.FindGameObjectWithTag("Player") }).ToArray();
    }

    private void FixedUpdate()
    {
        GameObject closestTarget = FindClosestTarget();
        if (closestTarget != null)
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
                GameObject player = GameObject.FindGameObjectWithTag("Player");
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
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject target in targets)
        {
            if (target != null && target != gameObject) // Kendi kendini hedef olarak alma
            {
                float distance = Vector3.Distance(target.transform.position, currentPosition);
                if (distance < closestDistance)
                {
                    closestTarget = target;
                    closestDistance = distance;
                }
            }
        }

        return closestTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetBool("isFloating", true);

            forceHit = UnityEngine.Random.Range(minFloat, maxFloat);
            Vector3 moveDirection = enemyRb.transform.position - collision.transform.position;
            enemyRb.AddForce(moveDirection.normalized * forceHit);

            StartCoroutine(WaitForAnimEnded());
        }
    }

    IEnumerator WaitForAnimEnded()
    {
        yield return new WaitForSeconds(.5f);
        animator.SetBool("isFloating", false);
    }
}
