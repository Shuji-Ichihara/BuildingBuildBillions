using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownBill2P : MonoBehaviour
{
    public GameObject[] Bills2P;
    private GameObject obj = null;
    public GameObject Obj => obj;

    // Start is called before the first frame update
    void Start()
    {
        NewBill2P();
    }


    public void NewBill2P()
    {
        if (Bills2P.Length != 0)
            obj = Instantiate(Bills2P[Random.Range(0, Bills2P.Length)], transform.position, Quaternion.identity);
    }
}
