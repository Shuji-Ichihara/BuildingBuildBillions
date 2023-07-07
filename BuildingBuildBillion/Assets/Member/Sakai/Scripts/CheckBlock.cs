using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock : MonoBehaviour
{
    NewBuildingcon bill;

    void Start()
    {
        bill = GetComponentInParent<NewBuildingcon>();


    }

    private void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("stage"))
        {
            bill.Stop = true;
        }
        if (collision.gameObject.CompareTag("Bill"))
        {
            bill.BuildingStop = true;
        }
        if (collision.gameObject.CompareTag("Bill2"))
        {
            bill.BuildingStop = true;
        }
    }

}
