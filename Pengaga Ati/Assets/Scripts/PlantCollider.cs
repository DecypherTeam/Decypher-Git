using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples
{
    public class PlantCollider : MonoBehaviour
    {
        public void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                Debug.Log("Player Pick");
                Player.pickedItem = false;
            }
        }
    }
}
