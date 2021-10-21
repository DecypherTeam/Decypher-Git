using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples
{
    public class CropsManager : MonoBehaviour
    {
        public GameObject[] Crops;

        GrowingCrop growingCrop;

        // Start is called before the first frame update
        void Start()
        {
            GameObject theCrop = GameObject.Find("Crops");
            growingCrop = theCrop.GetComponent<GrowingCrop>();
        }

        // Update is called once per frame
        void Update()
        {
            if (growingCrop.cropVisible == true)
            {
                Crops[01].transform.tag = "Untagged";
                Crops[01].SetActive(false);
                Crops[02].transform.tag = "Untagged";
                Crops[02].SetActive(false);
            }
        }
    }
}

