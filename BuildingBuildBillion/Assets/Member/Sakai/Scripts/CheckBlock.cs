using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock : MonoBehaviour
{
    NewBillcon bill;
    void Start()
    {
        bill = GetComponentInParent<NewBillcon>();
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
            bill.billstop = true;
        }
        if (collision.gameObject.CompareTag("Bill2"))
        {
            bill.billstop = true;
        }
    }

}
