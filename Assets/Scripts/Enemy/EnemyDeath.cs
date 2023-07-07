using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeath : MonoBehaviour
{
    private NavMeshAgent nva;
    private Animator animator;
    [SerializeField] SphereCollider sphereCollider;

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            animator.SetTrigger("Death");
            nva.isStopped = true;
            sphereCollider.enabled = false;
        }
    }
}
