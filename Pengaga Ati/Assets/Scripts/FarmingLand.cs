using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples
{
    public class FarmingLand : MonoBehaviour
    {
        public GameObject select;

        //public Rigidbody pickItem;
        //public Transform placementDest;

        public void Select(bool toggle)
        {
            select.SetActive(toggle);
        }

        /*public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "PickUp")
            {
                Debug.Log("planted");
                Destroy(pickItem);
            }
        }*/
    }
}

