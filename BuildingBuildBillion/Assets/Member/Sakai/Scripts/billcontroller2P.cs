﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class billcontroller2P : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Bill2"))
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
        if (collision.gameObject.CompareTag("Bill2"))
        {

            billstop = true;
        }
    }

    void FixedUpdate()
    {
        if (Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -50.0f, 0);
            previousTime = Time.time;
        }
        if (Stop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            //transform.position = new Vector3(transform.position.x, 1, 0);//座標を(0,1)に戻す
            this.enabled = false;
        }
        if (billstop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);//座標をその場にとどまる

            this.enabled = false;
        }

        fromMoveHorizonal += Time.deltaTime;

        if (fromMoveHorizonal >= restTime)
        {
            pad = true;
            fromMoveHorizonal = 0.0f;
        }

    }

    public void L_Rotation()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
    }
    public void R_Rotation()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
    }
    //if (Input.GetButtonDown("Rotate_right_2P"))
    //        //{
    //        //    Debug.Log("yyye");
    //        //    transform.Rotate(0, 0, RotateAxis);
    //        //}
    //        //if (Input.GetButtonDown("Rotate_left_2P"))
    //        //{
    //        //    transform.Rotate(0, 0, -RotateAxis);
    //        //}          
    /*
     if (Input.GetKeyDown(KeyCode.LeftArrow))
     {
         var screenPoint = CameraControllerTest.Instance.Camera.WorldToViewportPoint(this.transform.position + new Vector3(-1, 0, 0));// 0,0~1.1

         //if (screenPoint.x >= 0.51 && screenPoint.x <= 1)
         //    transform.position += new Vector3(-1, 0, 0);

         // 市原が記述
         // 要変更
         if (CameraControllerTest.Instance.Camera.orthographicSize < 1080.0f * 1.5f)
         {
             if (screenPoint.x >= 0.55f && screenPoint.x <= 0.8f)
                 transform.position += new Vector3(-50.0f, 0, 0);
         }
         if (screenPoint.x > 0.55f)
             transform.position += new Vector3(-50.0f, 0, 0);
     }
     else if (Input.GetKeyDown(KeyCode.RightArrow))
     {
         var screenPoint = CameraControllerTest.Instance.Camera.WorldToViewportPoint(this.transform.position + new Vector3(1, 0, 0));// 0,0~1.1

         // 市原が記述
         // 要変更
         if (CameraControllerTest.Instance.Camera.orthographicSize < 1080.0f * 1.5f)
         {
             if (screenPoint.x >= 0.55 && screenPoint.x <= 0.8f)
                 transform.position += new Vector3(50.0f, 0, 0);
         }
         if (screenPoint.x < 1.0f)
             transform.position += new Vector3(50.0f, 0, 0);
     }
 }
}
    */
    /// <summary>
    /// ブロック移動関数
    /// </summary>
    /// <param name="inputhorizotal"></param>
    /// <param name="inputvertical"></param>
    /// <param name="Player"> false =1p true=2p</param>
    public void BillMovememt(InputAction.CallbackContext context)
    {
        var k = context.ReadValue<Vector2>();
        Debug.Log(k);
        if (pad == true && k.x * k.x >= 0.25f)
        {
            float moveDistance = 50.0f;
            if (k.x < 0) moveDistance *= -1;
            var screenPoint = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(moveDistance, 0, 0));// 0,0~1.1

            if (CameraControllerTest.Instance.Camera.orthographicSize < 1080.0f * 1.5f)
            {
                if (screenPoint.x >= 0.55 && screenPoint.x <= 0.8)
                    transform.position += new Vector3(moveDistance, 0, 0);
            }
            transform.position += new Vector3(moveDistance, 0, 0);

            pad = false;

        }

        // 自動で下に移動させつつ、下矢印キーでも移動する
        if (pad == true && k.y * k.y >= 0.25f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (k.y <= 0.25f)
            {
                transform.position += new Vector3(0, Mathf.Sign(k.y) * 50.0f, 0);
                pad = false;
            }
        }

        if (billstop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            // transform.position = new Vector3(transform.position.x, transform.position.y, 0);//座標をその場にとどまる
            FindObjectOfType<SpownBill2P>().NewBill2P();//新しいビルをリスポーン
        }

    }
    void OnDisable()
    {
        this.GetComponent<PlayerInput>().enabled = false;
        FindObjectOfType<SpownBill2P>().NewBill2P();//新しいビルをリスポーン
    }

    public void FreezeAll(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}