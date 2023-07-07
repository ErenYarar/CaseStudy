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
    private Vector3 initialPosition;

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        initialPosition = transform.position;

        // Hedefleri ayarla: Oyuncu ve diğer düşmanlar
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        targets = targets.Concat(new[] { GameObject.FindGameObjectWithTag("Player") }).ToArray();
    }

    private void FixedUpdate()
    {
        GameObject closestTarget = FindClosestTarget();
        if (closestTarget != null)
        {
            // En yakın hedefe doğru hareket et
            nva.destination = closestTarget.transform.position;
        }
    }

    private GameObject FindClosestTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject target in targets)
        {
            if (target != gameObject) // Kendi kendini hedef olarak alma
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
            Vector3 moveDirection = enemyRb.transform.position - collision.transform.position;
            enemyRb.AddForce(moveDirection.normalized * 1000f);
            StartCoroutine(WaitForAnimEnded());
        }
    }

    IEnumerator WaitForAnimEnded()
    {
        yield return new WaitForSeconds(.5f);
        animator.SetBool("isFloating", false);
    }
}
