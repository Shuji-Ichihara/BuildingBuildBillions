using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBillcon : MonoBehaviour
{
    [SerializeField] private float DownSpeed = 10;
    public float previousTime;
    // ブロックの落ちる時間
    public float fallTime = 1f;
    private bool _isHorizontalPressed = false;
    [SerializeField]
    public float Rotateangle = 90;
    [SerializeField]
    public GameObject col;
    [SerializeField]
    public GameObject col2;
    public enum PlayerNum
    {
        Player1 = 0,
        Player2 = 1
    }
    [SerializeField]
    Collider2D Delete;
    public PlayerNum Player;

    public bool _isRightPressed = false;
    public bool _isLeftPressed = false;
    public bool _isHozirontalPressed = false;
    // ブロック回転
    public Vector3 rotationPoint;
    public bool Stop;
    public float stoptime = 0;
    private Vector2 _inputMove = Vector2.zero;
    //private bool rightwall;
    //private bool leftwall;
    public bool billstop;
    Rigidbody2D rb;
    bool pad = false;
    public bool Rotatepermission = false;

    [SerializeField]
    float restTime = 0.25f;
    float restTime2 = 0.25f;
    public float RotaterestTime = 0.5f;
    float fromMoveHorizonal = 0.0f;
    public float fromRotate = 0.0f;

    public bool Right = true;
    public bool Left = true;
    public float ColPoint = 50;
    public bool ColStop = false;
    private Vector3 screenPoint;
    private Vector3 Billposi = new Vector3(0, -50f, 0);
    List<Vector3> pastPositions = new List<Vector3>();
    public bool test = false;

    //public Vector3 billControllerPosition { get; private set; }

    void Start()
    {

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.SetActive(true);
        col2.SetActive(false);
        this.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("stage"))
    //    {
    //        Stop = true;
    //        this.GetComponent<PlayerInput>().enabled = false;
    //    }
    //    if (collision.gameObject.CompareTag("Bill"))
    //    {

    //        billstop = true;
    //        this.GetComponent<PlayerInput>().enabled = false;
    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("stage"))
    //    {
    //        Stop = true;
    //        this.GetComponent<PlayerInput>().enabled = false;
    //        rb.isKinematic = false;
    //        Delete.enabled = false;
    //    }
    //    if (collision.gameObject.CompareTag("Bill"))
    //    {
    //        ColPoint = collision.gameObject.transform.position.y;
    //        //billstop = true;
    //        rb.isKinematic = false;
    //        this.GetComponent<PlayerInput>().enabled = false;

    //        Delete.enabled = false;
    //    }
    //    if (collision.gameObject.CompareTag("Bill2"))
    //    {
    //        ColPoint = collision.gameObject.transform.position.y;
    //        //billstop = true;
    //        rb.isKinematic = false;
    //        this.GetComponent<PlayerInput>().enabled = false;
    //        Delete.enabled = false;
    //    }
    //}

    void Update()
    {
        if (_inputMove.x == 0 && _inputMove.y == 0)
        {
            Left = true;
            Right = true;
        }



        // 現在の座標を取得してリストに追加
        Vector3 currentPosition = transform.position;
        pastPositions.Add(currentPosition);
        int framesToGoBack = 10;


        if (Right == false || Left == false)
        {

            ReturnToPastPosition(framesToGoBack);

        }

        int maxPositions = 15;
        if (pastPositions.Count > maxPositions)
        {
            pastPositions.RemoveAt(0);
        }

        if (Stop == true || billstop == true)
        {
            //Debug.Log(Stop);
            //Debug.Log(billstop);
            this.GetComponent<PlayerInput>().enabled = false;
            this.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 400;
            //rb.isKinematic = false;
        }

        stoptime += Time.deltaTime;
        //if (stoptime >= 5.0f || rb.isKinematic == false)
        //{
        //    checkstop();
        //}

        //if (Stop == true)
        //{
        //    this.GetComponent<PlayerInput>().enabled = false;
        //    rb.isKinematic = false;
        //    Delete.enabled = false;
        //}
        //if (billstop == true)
        //{

        //    rb.isKinematic = false;
        //    this.GetComponent<PlayerInput>().enabled = false;

        //    Delete.enabled = false;
        //}
        //if (collision.gameObject.CompareTag("Bill2"))
        //{
        //    ColPoint = collision.gameObject.transform.position.y;
        //    //billstop = true;
        //    rb.isKinematic = false;
        //    this.GetComponent<PlayerInput>().enabled = false;
        //    Delete.enabled = false;
        //}
        switch (Player)
        {
            case PlayerNum.Player1:
                if (screenPoint.x <= 0.02f || screenPoint.x >= 0.45f)
                {

                    Left = false;
                    Debug.Log(Left);
                }
                else if (screenPoint.x >= 0.02f && screenPoint.x <= 0.45f)
                {

                    if (_inputMove.y == 0 && _inputMove.x != 0)
                    {

                        Left = true;

                    }
                    else if (_inputMove.y != 0 && _inputMove.x != 0)
                    {

                        Left = true;

                    }
                    else if (_inputMove.x == 0 && _inputMove.y != 0)
                    {

                        Left = true;

                    }
                    else if (_inputMove.x == 0 && _inputMove.y == 0)
                    {

                        Left = true;

                    }
                    Left = true;
                }

                break;
            case PlayerNum.Player2:
                if (screenPoint.x >= 0.55f && screenPoint.x <= 0.95f)
                {
                    if (_inputMove.y == 0 && _inputMove.x != 0)
                    {
                        Right = true;
                    }
                    Right = true;
                }
                if (screenPoint.x <= 0.55f || screenPoint.x >= 0.95f)
                {
                    Right = false;
                }

                break;
        }

        fromRotate += Time.deltaTime;

        if (fromRotate >= RotaterestTime)
        {
            Rotatepermission = true;
            fromRotate = 0.0f;
        }
        if (Rotatepermission == true)
        {
            if (_isLeftPressed)
            {
                test = true;
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -Rotateangle);


                if (col.activeSelf == false)
                {
                    Debug.Log("haitteruuuuuu");
                    col.SetActive(true);
                    col2.SetActive(false);
                }
                else if (col.activeSelf == true)
                {
                    Debug.Log("haitteru");
                    col.SetActive(false);
                    col2.SetActive(true);
                }



                test = false;
            }
            if (_isRightPressed)
            {
                test = true;
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), Rotateangle);

                if (col.activeSelf == true)
                {
                    Debug.Log("haitteruuuuuu");
                    col.SetActive(false);
                    col2.SetActive(true);
                }
                else if (col.activeSelf == false)
                {
                    Debug.Log("haitteru");
                    col.SetActive(true);
                    col2.SetActive(false);
                }


                test = false;
            }
            Rotatepermission = false;
        }

        if (pad == true && _inputMove.x * _inputMove.x >= 0.25f)
        {
            Vector2 moveDistance = new Vector2(50.0f, 0);
            if (_inputMove.x < 0) moveDistance *= -1;

            screenPoint = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(moveDistance.x, 0, 0));// 0,0~1.1

            if (CameraControllerTest.Instance.Camera.orthographicSize < 1080.0f * 1.5f)
            {
                if (Right || Left)
                {
                    if (Mathf.Ceil(_inputMove.x) == -1)
                    {
                        _isHorizontalPressed = false;
                        //transform.position += new Vector3(moveDistance, 0, 0);
                        //Vector2 Move = this.transform.position + moveDistance;
                        //rb.MovePosition(Move);
                        rb.velocity += moveDistance * 20;
                    }
                    if (Mathf.Ceil(_inputMove.x) == 1)
                    {
                        _isHorizontalPressed = false;
                        //Vector2 Move = this.transform.position + moveDistance;
                        //rb.MovePosition(Move);
                        rb.velocity += moveDistance * 20;
                    }
                }

                pad = false;
                //Left = false;
                //Right = false;
                stoptime = 0.0f;
            }
        }

        if (Time.time - previousTime >= fallTime)
        {

            transform.position += Billposi;
            previousTime = Time.time;
            stoptime = 0.0f;
            Right = true;
            Left = true;

        }
        if (Stop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            //transform.position = new Vector3(transform.position.x, 1, 0);//座標を(0,1)に戻す
            this.enabled = false;
            CreateNextBlock();
        }
        if (billstop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);//座標をその場にとどまる

            this.enabled = false;
            CreateNextBlock();
        }

        fromMoveHorizonal += Time.deltaTime;

        if (fromMoveHorizonal >= restTime)
        {
            pad = true;
            fromMoveHorizonal = 0.0f;
        }
        if (Mathf.Ceil(_inputMove.y) == -1)
        {

            rb.velocity += new Vector2(0, Mathf.Abs(_inputMove.y) * -DownSpeed * 20);
            Right = true;
            Left = true;

        }


    }

    public void _Rotate(InputAction.CallbackContext context)
    {
        var y = context.control.name;
        //Debug.Log(y);
        switch (context.phase)
        {
            case InputActionPhase.Performed:

                if (y == "leftShoulder" || y == "r" || y == "n")
                {
                    _isLeftPressed = true;
                }
                if (y == "rightShoulder" || y == "t" || y == "m")
                {
                    _isRightPressed = true;
                }
                break;
            case InputActionPhase.Canceled:
                _isLeftPressed = false;
                _isRightPressed = false;
                break;
        }
    }
    public void OnHold(InputAction.CallbackContext context)
    {
        _inputMove = context.ReadValue<Vector2>();
        //Debug.Log(_inputMove);
        _isHorizontalPressed = true;
    }

    /// <summary>
    /// 次のブロックの生成ロジック
    /// </summary>
    void CreateNextBlock()
    {
        switch (Player)
        {
            case PlayerNum.Player1:
                this.GetComponent<PlayerInput>().enabled = false;
                FindObjectOfType<SpownBill>().NewBill();//新しいビルをリスポーン
                                                        // このオブジェクトが無効化される時に、自身の座標を格納
                GameManager.Instance.SpownBill.BuildingPosition = transform.position;
                break;
            case PlayerNum.Player2:
                this.GetComponent<PlayerInput>().enabled = false;
                FindObjectOfType<SpownBill2P>().NewBill2P();//新しいビルをリスポーン
                                                            // このオブジェクトが無効化される時に、自身の座標を格納
                GameManager.Instance.SpownBill.BuildingPosition = transform.position;
                break;

                //this.GetComponent<PlayerInput>().enabled = false;
                //FindObjectOfType<SpownBill>().NewBill();//新しいビルをリスポーン
                //                                        // このオブジェクトが無効化される時に、自身の座標を格納
                //GameManager.Instance.SpownBill.BuildingPosition = transform.position;
        }
    }

    void ReturnToPastPosition(int frameCount)
    {
        int targetIndex = pastPositions.Count /*- 1*/ - frameCount;
        if (targetIndex >= 0 && targetIndex < pastPositions.Count)
        {
            Vector3 targetPosition = pastPositions[targetIndex];
            this.transform.position = targetPosition;


            Right = true;
            Left = true;

        }
    }
    /// <summary>
    /// ああああああ
    /// </summary>
    /// <returns></returns>
    //void FixedUpdate()
    //{
    //    while (true)
    //    {
    //        Debug.Log("yoba");
    //        //ブロックの過去のY座標を取得
    //        //数秒停止

    //        //今の座標を取得」  
    //        Vector3 posinow = this.transform.localPosition;　//今の座標
    //        yield return new WaitForSeconds(2);
    //        Vector3 posipast= this.transform.position; //過去座標
    //        Debug.Log("hatu");
    //        //今の座標と過去の座標を比較比較的大きくして
    //        if(posinow.y == posipast.y)
    //        {
    //            billstop = true;
    //        }
    //        //スクリプトをエナブル
    //        this.enabled = false;
    //        yield return new WaitForSeconds(2);
    //    }

    //}
    //private void checkstop()
    //{
    //    Debug.Log("yobare");
    //    if (rb.IsSleeping())
    //    {
    //        Debug.Log("true");
    //        billstop = true;
    //        this.enabled = false;
    //    }
    //    else
    //    {
    //        Debug.Log("false");
    //        return;
    //    }
    //}

}

