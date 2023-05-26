using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownBill : MonoBehaviour
{
    public GameObject[] Bills;
    private GameObject obj = null;
    public GameObject Obj => obj;

    // Start is called before the first frame update
    void Start()
    {
        NewBill();

    }

    public void NewBill()
    {
        if (Bills.Length != 0)
            obj = Instantiate(Bills[Random.Range(0, Bills.Length)], transform.position, Quaternion.identity);
    }

}
