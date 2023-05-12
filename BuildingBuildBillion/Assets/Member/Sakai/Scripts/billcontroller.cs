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
    void Update()
    {
        BillMovememt();
    }
    
    private void BillMovememt()
    {
        // �����L�[�ō��ɓ���
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        // �E���L�[�ŉE�ɓ���
         if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        //���̕ǂɓ����������ɒl��߂�
        if (transform.position.x < -8)
        {
            leftwall = true;
        }
        if (leftwall == true)
        {
            transform.position = new Vector3(-8,transform.position.y, 0);
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
         if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
           

        }

        
        if (Stop == true)
        {
            transform.position = new Vector3(transform.position.x, 1, 0);//���W��(0,1)�ɖ߂�
          
            FindObjectOfType<SpownBill>().NewBill();//�V�����r�������X�|�[��
           
            this.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �u���b�N������L�[�������ĉ�]������
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }

        if (billstop == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);//���W�����̏�ɂƂǂ܂�

            FindObjectOfType<SpownBill>().NewBill();//�V�����r�������X�|�[��
           
            this.enabled = false;
        }
    }
}