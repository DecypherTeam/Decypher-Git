using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Examples
{
    public class PlayerInteraction : MonoBehaviour
    {
        Player player;

        FarmingLand selectedLand = null;

        void Start()
        {
            player = transform.parent.GetComponent<Player>();
        }

        void Update()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, 2))
            {
                OnInteractableHit(hit);
            }
        }

        // Handles what happens when the interactor raycast hits farming lands
        void OnInteractableHit(RaycastHit hit)
        {
            // Check if the player is going to interact with land
            Collider other = hit.collider;
            if (other.tag == "Land")
            {
                // Get the farming land component
                FarmingLand land = other.GetComponent<FarmingLand>();
                SelectLand(land);
                return;
            }

            // Unselect the land if the player is not standing on any land at the moment
            if (selectedLand != null)
            {
                selectedLand.Select(false);
                selectedLand = null;
            }
        }

        // Handles the selection process
        void SelectLand(FarmingLand land)
        {
            //Set the previously selected land to false (If any)
            if (selectedLand != null)
            {
                selectedLand.Select(false);
            }

            // Set the new selected land to the land we're selecting now.
            selectedLand = land;
            land.Select(true);
        }
    }
}

