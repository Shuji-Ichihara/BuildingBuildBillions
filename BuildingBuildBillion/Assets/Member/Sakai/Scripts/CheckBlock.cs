using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock : MonoBehaviour
{
    NewBillcon bill;
    void Start()
    {
        //bill = transform.GetChild(0).gameObject;/*.GetComponent<NewBillcon>().enabled = true;*/
        bill = GetComponent<NewBillcon>();
        Debug.Log(bill.name);
    }

    private void Update()
    {
        //bill.BlockMove();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bill1"))
        {

            //bill.billstop = true;
            //bill.gameObject.GetComponent<NewBillcon>().billstop = false;
            bill.billstop = true;

        }
        if (collision.gameObject.CompareTag("stage"))
        {
           bill. Stop = true;
        }
    }

}
