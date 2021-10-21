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
        // Variable for the script, Player.cs
        Player player;

        Rigidbody rb;

        public GameObject mesh;
        public Material ghostMaterial;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            target = GameObject.FindGameObjectWithTag("Plant");

            animator = GetComponent<Animator>();

            rb = GetComponent<Rigidbody>();

            // Reference to the script that holds the crops which is GrowingCrop.cs
            GameObject theCrop = GameObject.Find("Crops");
            growingCrop = theCrop.GetComponent<GrowingCrop>();

            // Reference to the script that holds the player which is Player.cs
            GameObject thePlayer = GameObject.Find("Player");
            player = thePlayer.GetComponent<Player>();
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
                /*Debug.Log("Crop destroyed");*/
            }

            // Trying to create bullet making contact with enemy and enemy die
            /*float bulletdist = Vector3.Distance(transform.position, player.game.transform.position);
            if (bulletdist < stoppingDistance)
            {
                Debug.Log("Bullet hit");
            }*/
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

        public IEnumerator WaitBeforeGhost()
        {
            yield return new WaitForSeconds(2);
            mesh.GetComponent<Renderer>().material = ghostMaterial;
            rb.useGravity = false;
            agent.baseOffset = 2f;
            agent.speed = 2f;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Sphere")
            {
                animator.SetBool("isDead", true);
                agent.speed = 0f;
                StartCoroutine(WaitBeforeDie());
            }
        }

        public IEnumerator WaitBeforeDie()
        {
            yield return new WaitForSeconds(2);
            StartCoroutine(WaitBeforeGhost());
        }
    }
}

