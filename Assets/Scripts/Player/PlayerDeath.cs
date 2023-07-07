using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDeath : MonoBehaviour
{
    private NavMeshAgent nva;
    private Animator animator;
    bool isDying = false;
    float deathTimer = 0f;
    // public GameObject deathMenu;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject joystick;

    private void Start()
    {
        nva = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // private void Update()
    // {
    //     if (isDying)
    //     {
    //         deathTimer -= Time.deltaTime;
    //         if (deathTimer <= 0)
    //         {
    //             // deathMenu.SetActive(true);
    //             Time.timeScale = 0f;
    //         }
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border") && !isDying)
        {
            animator.SetTrigger("Death");
            nva.velocity = Vector3.zero;
            nva.isStopped = true;
            sphereCollider.enabled = false;
            isDying = true;

            playerMovement.enabled = false;
            joystick.SetActive(false);
            // deathTimer = 1f;
        }
    }
}
