using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float stoppingDistance;

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
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist < stoppingDistance)
        {
            StopEnemy();
            animator.SetBool("isEating", true);
        }
        else
        {
            GoToTarget();
        } 
    }

    private void GoToTarget()
    {
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
        animator.SetBool("isRunning", true);
    }

    private void StopEnemy()
    {
        agent.isStopped = true;
    }
}
