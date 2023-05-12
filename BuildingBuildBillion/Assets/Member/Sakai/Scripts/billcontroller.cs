using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // 左矢印キーで左に動く
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        // 右矢印キーで右に動く
         if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
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
         if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
           

        }

        
        if (Stop == true)
        {
            transform.position = new Vector3(transform.position.x, 1, 0);//座標を(0,1)に戻す
          
            FindObjectOfType<SpownBill>().NewBill();//新しいビルをリスポーン
           
            this.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ブロックを上矢印キーを押して回転させる
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }

        if (billstop == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);//座標をその場にとどまる

            FindObjectOfType<SpownBill>().NewBill();//新しいビルをリスポーン
           
            this.enabled = false;
        }
    }
}