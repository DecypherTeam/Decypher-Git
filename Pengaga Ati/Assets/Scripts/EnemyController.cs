using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Examples
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] float stoppingDistance;

        NavMeshAgent agent;

        GameObject target;

        Animator animator;

        // Variable for the script, GrowingCrop.cs
        GrowingCrop growingCrop;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            target = GameObject.FindGameObjectWithTag("Plant");

            animator = GetComponent<Animator>();

            // Reference to the script that holds the crops which is GrowingCrop.cs
            GameObject theCrop = GameObject.Find("Crops");
            growingCrop = theCrop.GetComponent<GrowingCrop>();
        }

        private void Update()
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < stoppingDistance)
            {
                StopEnemy();
                animator.SetBool("isEating", true);
                StartCoroutine(growingCrop.WaitBeforeDestroy());
            }
            else
            {
                GoToTarget();
            }

            if (growingCrop.cropDestroyed == true)
            {
                animator.SetBool("isEating", false);
                animator.SetBool("isRunning", false);
                Debug.Log("Crop destroyed");
            }
        }

        private void GoToTarget()
        {
            if(growingCrop.harvestReady == true)
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
                animator.SetBool("isRunning", true);
            }  
        }

        private void StopEnemy()
        {
            agent.isStopped = true;
        }
    }
}

