using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class billcontroller : MonoBehaviour
{
    public float previousTime;
    // ブロックの落ちる時間
    public float fallTime = 1f;

    // ブロック回転
    public Vector3 rotationPoint;
    private bool Stop;
    private bool rightwall;
    private bool leftwall;
    private bool billstop;

    Rigidbody2D rb;
    bool pad = false;

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
        BillMovememt(Input.GetAxisRaw("D_Pad_H"),Input.GetAxisRaw("D_Pad_V"));

        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.Log("ゲームパッドがありません。");
        
            return;
        }
       
    }
  
   
    private void BillMovememt(float inputhorizotal,float inputvertical)
    {
        Debug.Log(inputhorizotal);
        Debug.Log(pad);
        // 左矢印キーで左に動く
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    transform.position += new Vector3(-1, 0, 0);
        //}
        // 右矢印キーで右に動く
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        if(inputhorizotal !=0 || inputvertical !=0)
        {
            if (pad == false && inputhorizotal == 0)
            {
                pad = true;
            }
            if (pad == true && inputhorizotal != 0)
            {
                transform.position += new Vector3(inputhorizotal, inputvertical, 0);

                pad = false;
                //}
            }
        }
        
        //左の壁に当たった時に値を戻す
        if (transform.position.x < -8)
        {
            leftwall = true;
        }
        if (leftwall == true)
        {
            transform.position = new Vector3(-8,transform.position.y, 0);
            leftwall = false;
        }
        //右の壁に当たった時に値を戻す
        if (transform.position.x > 8)
        {
            rightwall = true;
        }
        if (rightwall == true)
        {
            transform.position = new Vector3(8, transform.position.y, 0);
            rightwall = false;
        }
        
            // 自動で下に移動させつつ、下矢印キーでも移動する
            if ( Time.time - previousTime >= fallTime)
            {
                transform.position += new Vector3(0, -1, 0);
                previousTime = Time.time;

            }
        
        if (Stop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, 1, 0);//座標を(0,1)に戻す
          
            FindObjectOfType<SpownBill>().NewBill();//新しいビルをリスポーン
           
            this.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            // ブロックを上矢印キーを押して回転させる
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }

        if (billstop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);//座標をその場にとどまる
            FindObjectOfType<SpownBill>().NewBill();//新しいビルをリスポーン
            


            this.enabled = false;
        }

    }
}