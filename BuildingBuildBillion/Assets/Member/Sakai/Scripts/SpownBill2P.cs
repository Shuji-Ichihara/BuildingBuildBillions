using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownBill2P : MonoBehaviour
{
    public GameObject[] Bills2P;
    public Vector3 BuildingPosition { get; set; }


    public void NewBill2P()
    {
        if (GameManager.Instance.CountDownGameTime < 0.0f) { return; }
        if (Bills2P.Length != 0)
            GameManager.Instance.Obj2 = Instantiate(Bills2P[Random.Range(0, Bills2P.Length)], transform.position, Quaternion.identity);
    }
}
