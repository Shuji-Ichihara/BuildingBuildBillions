using UnityEngine;

public class SpownBill : MonoBehaviour
{
    public GameObject[] Bills;
    private GameObject obj = null;
    public GameObject Obj => obj;
    public Vector3 BuildingPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        NewBill();
    }

    public void NewBill()
    {
        if (GameManager.Instance.CountDownTime < 0.0f) { return; }
        if (Bills.Length != 0)
            obj = Instantiate(Bills[Random.Range(0, Bills.Length)], transform.position, Quaternion.identity);
    }

}
