using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class billcontroller : MonoBehaviour
{
    [SerializeField] private float DownSpeed = 10;
    public float previousTime;
    // ブロックの落ちる時間
    public float fallTime = 1f;
    private bool _isHorizontalPressed = false;
    [SerializeField]
    float Rotateangle = 90;

    public enum PlayerNum
    {
        Player1 = 0,
        Player2 = 1
    }
    [SerializeField]
    Collider2D Delete;
    public PlayerNum jittai;

    private bool _isRightPressed = false;
    private bool _isLeftPressed = false;
    private bool _isHozirontalPressed = false;
    // ブロック回転
    public Vector3 rotationPoint;
    private bool Stop;

    private Vector2 _inputMove = Vector2.zero;
    //private bool rightwall;
    //private bool leftwall;
    private bool billstop;
    Rigidbody2D rb;
    bool pad = false;
    bool Rotatepermission = false;

    [SerializeField]
    float restTime = 0.25f;
    float restTime2 = 0.25f;
    float RotaterestTime = 0.25f;
    float fromMoveHorizonal = 0.0f;
    float fromRotate = 0.0f;

    public float ColPoint = 50;
    public bool ColStop = false;
    private Vector3 screenPoint;

    //public Vector3 billControllerPosition { get; private set; }

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (ColPoint == 50)
        {
            ColStop = true;
        }
        StartCoroutine(BlockStopOrDetection());
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("stage"))
        {
            Stop = true;
            this.GetComponent<PlayerInput>().enabled = false;
        }
        if (collision.gameObject.CompareTag("Bill"))
        {
            ColPoint = collision.gameObject.transform.position.x;
            billstop = true;
            if (ColStop == false)
            {
                this.GetComponent<PlayerInput>().enabled = false;
            }
        }
        if (collision.gameObject.CompareTag("Bill2"))
        {

            billstop = true;
            this.GetComponent<PlayerInput>().enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    void Update()
    {

        switch (jittai)
        {
            case PlayerNum.Player1:

                break;
            case PlayerNum.Player2:
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
                Debug.Log("RightRotate");
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -Rotateangle);
                Rotatepermission = false;
            }
            if (_isRightPressed)
            {
                Debug.Log("LeftRotate");
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), Rotateangle);
                Rotatepermission = false;
            }
        }

        if (pad == true && _inputMove.x * _inputMove.x >= 0.25f)
        {
            float moveDistance = 50.0f;
            if (_inputMove.x < 0) moveDistance *= -1;

            screenPoint = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(moveDistance, 0, 0));// 0,0~1.1

            if (CameraControllerTest.Instance.Camera.orthographicSize < 1080.0f * 1.5f)
                if (screenPoint.x >= 0f && screenPoint.x <= 0.45f)
                {
                    if (Mathf.Ceil(_inputMove.x) == -1)
                    {
                        Debug.Log("RightLeft");
                        _isHorizontalPressed = false;
                        transform.position += new Vector3(moveDistance, 0, 0);

                    }
                    if (Mathf.Ceil(_inputMove.x) == 1)
                    {
                        Debug.Log("RightLeft");
                        _isHorizontalPressed = false;
                        transform.position += new Vector3(moveDistance, 0, 0);

                    }
                }
            pad = false;

        }

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

        transform.position += new Vector3(0, Mathf.Abs(_inputMove.y) * -DownSpeed, 0);
        
    }

    public void _Rotate(InputAction.CallbackContext context)
    {
        var y = context.control.name;
        Debug.Log(y);
        switch (context.phase)
        {
            case InputActionPhase.Performed:

                if (y == "leftShoulder")
                {
                    _isLeftPressed = true;
                }
                if (y == "rightShoulder")
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
        Debug.Log(_inputMove);
        _isHorizontalPressed = true;
    }


    void OnDisable()
    {
        switch (jittai)
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

                this.GetComponent<PlayerInput>().enabled = false;
                FindObjectOfType<SpownBill>().NewBill();//新しいビルをリスポーン
                                                        // このオブジェクトが無効化される時に、自身の座標を格納
                GameManager.Instance.SpownBill.BuildingPosition = transform.position;
        }
    }

    public void FreezeAllConstraints(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    /// <summary>
    /// ああああああ
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlockStopOrDetection()
    {
        while(true)
        {
            Debug.Log("yoba");
            //ブロックの過去のY座標を取得
            //数秒停止
            yield return new WaitForSeconds(1);
            //今の座標を取得」  
            Debug.Log("hatu");
            //今の座標と過去の座標を比較比較的大きくして
            //スクリプトをエナブル
            yield return new WaitForSeconds(1);
        }
        
    }
}
