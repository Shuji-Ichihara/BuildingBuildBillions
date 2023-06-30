using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    public GameObject col;
    [SerializeField]
    public GameObject col2;
    // Start is called before the first frame update
    void Start()
    {
        col.SetActive(true);
        col2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Debug.Log("haitteruuuuuu");
            col.SetActive(false);
           col2.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            Debug.Log("haitteru");
          col.SetActive(true);
            col2.SetActive(false);
        }
    }
}
