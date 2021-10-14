using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolDest : MonoBehaviour
{
    public int pivotPoint;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC")
        {
            if (pivotPoint == 5)
            {
                pivotPoint = 0;
            }

            if (pivotPoint == 4)
            {
                this.gameObject.transform.position = new Vector3(14, -12, -30);
                pivotPoint = 5;
            }

            if (pivotPoint == 3)
            {
                this.gameObject.transform.position = new Vector3(-8, -12, -33);
                pivotPoint = 4;
            }

            if (pivotPoint == 2)
            {
                this.gameObject.transform.position = new Vector3(-24, -12, -33);
                pivotPoint = 3;
            }

            if (pivotPoint == 1)
            {
                this.gameObject.transform.position = new Vector3(-17, -12, -20);
                pivotPoint = 2;
            }

            if (pivotPoint == 0)
            {
                this.gameObject.transform.position = new Vector3(14, -12, -20);
                pivotPoint = 1;
            }
        }
    }
}
