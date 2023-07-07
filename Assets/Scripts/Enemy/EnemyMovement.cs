using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent nva;
    Rigidbody enemyRb;
    Transform player;
    private Animator animator;
    private Vector3 initialPosition;

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        nva.destination = player.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
