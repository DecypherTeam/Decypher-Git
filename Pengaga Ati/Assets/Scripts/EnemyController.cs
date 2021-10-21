using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;

    GameObject target;

    Animator animator;

    private void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Plant");

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GoToTarget();
    }

    private void GoToTarget()
    {
        agent.SetDestination(target.transform.position);
        animator.SetBool("isEating", true);
    }
}
