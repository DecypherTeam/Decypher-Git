using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples
{
    public class PlantCollider : MonoBehaviour
    {
        Player player;

        void Start()
        {
            GameObject thePlayer = GameObject.Find("Player");
            player = thePlayer.GetComponent<Player>();
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                //Debug.Log("Player Pick");
                player.pickedItem = false;
            }
        }
    }
}
