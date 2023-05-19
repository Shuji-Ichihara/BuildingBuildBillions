using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class billcontroller : MonoBehaviour
{
    public float previousTime;
    // �u���b�N�̗����鎞��
    public float fallTime = 1f;

    // �u���b�N��]
    public Vector3 rotationPoint;
    private bool Stop;
    private bool rightwall;
    private bool leftwall;
    private bool billstop;
    Rigidbody2D rb;
    bool pad = false;

    [SerializeField]
    float restTime = 0.25f;
    float fromMoveHorizonal = 0.0f;

    void Start()
    {

        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("stage"))
        {
            Stop = true;
        }
        if (collision.gameObject.CompareTag("Bill"))
        {

            billstop = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("stage"))
        {
            Stop = true;
        }
        if (collision.gameObject.CompareTag("Bill"))
        {

            billstop = true;
        }
    }

    void Update()
    {
        //Debug.Log(Stop);
        BillMovememt(Input.GetAxisRaw("D_Pad_H"), Input.GetAxisRaw("D_Pad_V"));

        Rotate(90);
        //BillMovememt(Input.GetAxisRaw("Ratate_right"), Input.GetAxisRaw("Rotate_left"));
        //if (Input.GetKeyDown("joystick button 4"))
        //{
        //    Debug.Log("button4");
        //}
        //if (Input.GetKeyDown("joystick button 5"))
        //{
        //    Debug.Log("button5");
        //}

        fromMoveHorizonal += Time.deltaTime;

        if (fromMoveHorizonal >= restTime)
        {
            pad = true;
            fromMoveHorizonal = 0.0f;
        }

    }

    private void Rotate(int RotateAxis, bool cannon = false)
    {

        if (cannon == true)
        {
            if (Input.GetKey("joystick button 4"))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), RotateAxis);
            }
            if (Input.GetKey("joystick button 5"))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -RotateAxis);
            }
        }
        else if (cannon == false)
        {
            Debug.Log("false1");
            if (Input.GetButtonDown("Rotate_right_1P"))
            {
                Debug.Log("r1");
                transform.Rotate(0, 0, RotateAxis);
                //transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, RotateAxis);
            }
            if (Input.GetButtonDown("Rotate_left_1P"))
            {
                transform.Rotate(0, 0, -RotateAxis);
            }
            //�L�[�{�[�h���͂��������
        }
    }

    /// <summary>
    /// �u���b�N�ړ��֐�
    /// </summary>
    /// <param name="inputhorizotal"></param>
    /// <param name="inputvertical"></param>
    /// <param name="Player"> false =1p true=2p</param>
    private void BillMovememt(float inputhorizotal, float inputvertical)
    {
        if (pad == true && inputhorizotal * inputhorizotal >= 0.25f)
        {
            float moveDistance = 1.0f;
            if (inputhorizotal < 0) moveDistance *= -1;

            transform.position += new Vector3(moveDistance, 0, 0);

            pad = false;

        }

        //���̕ǂɓ����������ɒl��߂�
        if (transform.position.x < -8 || transform.position.x < 0)
        {
            leftwall = true;
        }
        if (leftwall == true)
        {
            transform.position = new Vector3(-8, transform.position.y, 0);
            leftwall = false;
        }
        //�E�̕ǂɓ����������ɒl��߂�
        if (transform.position.x > 8)
        {
            rightwall = true;
        }
        if (rightwall == true)
        {
            transform.position = new Vector3(8, transform.position.y, 0);
            rightwall = false;
        }

        // �����ŉ��Ɉړ������A�����L�[�ł��ړ�����
        if (pad == true && inputvertical * inputvertical >= 0.25f || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;

            pad = false;
        }

        if (Stop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, 1, 0);//���W��(0,1)�ɖ߂�

            FindObjectOfType<SpownBill>().NewBill();//�V�����r�������X�|�[��

            this.enabled = false;
        }
        //else if (Rtri > 0)
        //{
        //    // �u���b�N������L�[�������ĉ�]������
        //    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        //}

        if (billstop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);//���W�����̏�ɂƂǂ܂�
            FindObjectOfType<SpownBill>().NewBill();//�V�����r�������X�|�[��



            this.enabled = false;
        }

    }
}