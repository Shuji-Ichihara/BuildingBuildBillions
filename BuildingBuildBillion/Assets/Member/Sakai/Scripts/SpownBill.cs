using UnityEngine;

public class SpownBill : MonoBehaviour
{
    public GameObject[] Bills;
    public Vector3 BuildingPosition { get; set; }

    public void NewBill()
    {
        if (GameManager.Instance.CountDownGameTime < 0.0f) { return; }
        if (Bills.Length != 0)
            GameManager.Instance.Obj = Instantiate(Bills[Random.Range(0, Bills.Length)], transform.position, Quaternion.identity);
    }

}
