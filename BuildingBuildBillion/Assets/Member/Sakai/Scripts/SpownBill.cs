using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownBill : MonoBehaviour
{
    public GameObject[] Bills;
    // Start is called before the first frame update
    void Start()
    {
        NewBill();
    }

    public void NewBill()
    {
        Instantiate(Bills[Random.Range(0, Bills.Length)], transform.position, Quaternion.identity);
    }
}
