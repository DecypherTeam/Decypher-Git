using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Examples
{
    public class PlayerInteraction : MonoBehaviour
    {
        Player player;

        void Start()
        {
            player = transform.parent.GetComponent<Player>();
        }

        void Update()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, 1))
            {
                OnInteractableHit(hit);
            }
        }

        void OnInteractableHit(RaycastHit hit)
        {
            Collider other = hit.collider;
            if (other.tag == "Land")
            {
                FarmingLand land = other.GetComponent<FarmingLand>();
                land.Select(true);
            }
        }

    }
}

