using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples
{
    public class PlantInteraction : MonoBehaviour
    {
        public static bool isDestroyed = false;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Land")
            {
                Debug.Log("planted");
                Destroy(gameObject);
                isDestroyed = true;
            }
        }
    }
}
