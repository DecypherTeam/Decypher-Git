using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples
{
    public class PlantInteraction : MonoBehaviour
    {
        public bool isDestroyed = false;

        public bool land0;
        public bool land1;
        public bool land2;
        public bool land3;
        public bool land4;
        public bool land5;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Land")
            {
                /*Debug.Log("land0 = true");*/
                Destroy(gameObject);
                isDestroyed = true;
                land0 = true;
            }
            if (collision.gameObject.tag == "Land 1")
            {
                /*Debug.Log("land1 = true");*/
                Destroy(gameObject);
                isDestroyed = true;
                land1 = true;
            }
            if (collision.gameObject.tag == "Land 2")
            {
                /*Debug.Log("land2 = true");*/
                Destroy(gameObject);
                isDestroyed = true;
                land2 = true;
            }
            if (collision.gameObject.tag == "Land 3")
            {
                /*Debug.Log("land3 = true");*/
                Destroy(gameObject);
                isDestroyed = true;
                land3 = true;
            }
            if (collision.gameObject.tag == "Land 4")
            {
                /*Debug.Log("land4 = true");*/
                Destroy(gameObject);
                isDestroyed = true;
                land4 = true;
            }
            if (collision.gameObject.tag == "Land 5")
            {
                /*Debug.Log("land5 = true");*/
                Destroy(gameObject);
                isDestroyed = true;
                land5 = true;
            }
        }
    }
}
