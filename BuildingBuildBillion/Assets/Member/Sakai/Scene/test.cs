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
    public Rigidbody2D rb;

    void Start()
    {
        // Rigidbody�R���|�[�l���g���擾����
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //  Up�L�[�őO�ɐi��
        if (Input.GetKey("up"))
        {
            rb.AddForce(transform.right * 10.0f, ForceMode2D.Force);
        }
    }
}