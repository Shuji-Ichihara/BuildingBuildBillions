﻿using Cysharp.Threading.Tasks;
using SpriteGlow;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class NewBuildingcon : MonoBehaviour
{
    [SerializeField] private float _downSpeed = 250;
    private float _previousTime = 0;
    // ブロックの落ちる時間
    private float _fallTime = 1f;
    [SerializeField]
    private float _rotateAngle;
    [SerializeField]
    private GameObject _col = null;
    [SerializeField]
    private GameObject _col2 = null;
    private enum PlayerNum
    {
        Player1 = 0,
        Player2 = 1
    }
    private enum BlockStae
    {
        Normal,
        Canon
    }
    [SerializeField]
    private PlayerNum Player;
    [SerializeField] private BlockStae Block;

    private bool _isRightPressed = false;
    private bool _isLeftPressed = false;
    // ブロック回転
    private Vector3 _rotationPoint;
    private Vector2 _inputMove = Vector2.zero;

    private bool _pad = false;
    private bool _rotatePermission = false;

    [SerializeField]
    private float _restTime = 0.25f;
    private float _fromMoveHorizonal = 0.0f;

    private bool _right = true;
    private bool _left = true;
    private Vector3 _screenPoint;
    private Vector3 _buildingPosi = new Vector3(0, -50f, 0);
    [SerializeField]
    List<float> _pastPositions = new List<float>();
    private Vector2 _moveDistance = new Vector2(50.0f, 0);

    [SerializeField] private UnityEvent myEvent = new UnityEvent();
    public Sprite[] sprites; // スプライトの配列

    private SpriteRenderer spriteRenderer;

    public bool BuildingStop;
    public bool Stop;
    public bool isopareton;
    public Rigidbody2D rb;
    //public Vector3 billControllerPosition { get; private set; }

    public void Start()
    {
        isopareton = true;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (_col != null && _col2 != null)
        {
            _col.SetActive(true);
            _col2.SetActive(false);

        }
        this.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        switch (Block)
        {
            case BlockStae.Normal:
                _rotateAngle = 90;
                break;
            case BlockStae.Canon:
                _rotateAngle = 5;
                break;
        }
    }

    void Update()
    {
        SpriteGlowEffect scriptA = this.GetComponent<SpriteGlowEffect>();
        _previousTime += Time.deltaTime;
        if (_previousTime >= _fallTime)
        {

            transform.position += _buildingPosi;
            _previousTime = 0;

        }

        _screenPoint = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(_moveDistance.x, 0, 0));// 0,0~1.1
        if (_inputMove.x == 0 && _inputMove.y == 0)
        {
            _left = true;
            _right = true;
        }
        int framesToGoBack = 1;


        if (_right == false || _left == false)
        {

            ReturnToPastPosition(framesToGoBack);
            return;
        }

        int maxPositions = 15;
        if (_pastPositions.Count > maxPositions)
        {
            _pastPositions.RemoveAt(0);
        }

        if (Stop == true || BuildingStop == true)
        {
           
            this.GetComponent<PlayerInput>().enabled = false;
            this.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 400;
           
        }

        switch (Player)//1Pと2pで別々の移動制御
        {
            case PlayerNum.Player1:
                if (_screenPoint.x <= 0.04f || _screenPoint.x >= 0.45f)
                {


                    _left = false;
                    rb.velocity = Vector2.zero;
                }
                else if (_screenPoint.x >= 0.04f && _screenPoint.x <= 0.45f)
                {
                    float currentPosition = this.transform.position.x;
                    _pastPositions.Add(currentPosition);
                    if (_inputMove.y == 0 && _inputMove.x != 0)
                    {

                        _left = true;

                    }
                    if (_inputMove.y != 0 && _inputMove.x != 0)
                    {

                        _left = true;

                    }
                    if (_inputMove.x == 0 && _inputMove.y != 0)
                    {

                        _left = true;

                    }
                    if (_inputMove.x == 0 && _inputMove.y == 0)
                    {

                        _left = true;

                    }
                    _left = true;
                }

                break;
            case PlayerNum.Player2:
                if (_screenPoint.x <= 0.55f || _screenPoint.x >= 1.0f)
                {


                    _right = false;
                    rb.velocity = Vector2.zero;

                }
                else if (_screenPoint.x >= 0.55f && _screenPoint.x <= 1.0f)
                {
                    float currentPosition = this.transform.position.x;
                    _pastPositions.Add(currentPosition);
                    if (_inputMove.y == 0 && _inputMove.x != 0)
                    {

                        _right = true;

                    }
                    if (_inputMove.y != 0 && _inputMove.x != 0)
                    {

                        _right = true;

                    }
                    if (_inputMove.x == 0 && _inputMove.y != 0)
                    {

                        _right = true;

                    }
                    if (_inputMove.x == 0 && _inputMove.y == 0)
                    {

                        _right = true;

                    }
                    _right = true;
                }

                break;
        }
        if (_isLeftPressed)
        {
            if (_rotatePermission == false || Block == BlockStae.Canon)
            {
                transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, -1), -_rotateAngle);
                if (_col != null)
                {
                    if (_col.activeSelf == false && _col != null)
                    {
                        _col.SetActive(true);
                        _col2.SetActive(false);
                    }
                    else if (_col.activeSelf == true && _col != null)
                    {
                        _col.SetActive(false);
                        _col2.SetActive(true);
                    }
                }
                _rotatePermission = true;
            }

        }
        if (_isRightPressed)
        {
            if (_rotatePermission == false || Block == BlockStae.Canon)//大砲の場合のみ1度ずつ回転させる
            {
                transform.RotateAround(transform.TransformPoint(_rotationPoint), new Vector3(0, 0, -1), _rotateAngle);
                if (_col != null)
                {
                    if (_col.activeSelf == true && _col != null)
                    {
                        _col.SetActive(false);
                        _col2.SetActive(true);
                    }
                    else if (_col.activeSelf == false && _col != null)
                    {
                        _col.SetActive(true);
                        _col2.SetActive(false);
                    }
                }
                _rotatePermission = true;
            }
        }

        if (_pad == true && _inputMove.x * _inputMove.x >= 0.25f)//横移動の制御
        {


            if (CameraController.Instance.Camera.m_Lens.OrthographicSize < 1080.0f * 1.5f)
            {
                if (_right || _left)
                {
                    if (Mathf.Ceil(_inputMove.x) == -1)
                    {
                       
                        rb.velocity -= _moveDistance * 30;
                    }
                    if (Mathf.Ceil(_inputMove.x) == 1)
                    {
                       
                        rb.velocity += _moveDistance * 30;
                    }
                }

                _pad = false;


            }
        }

        if (Stop == true)
        {
            if (myEvent != null)
            {
                //myEventに登録されている関数を実行
                myEvent.Invoke();
            }
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            //transform.position = new Vector3(transform.position.x, 1, 0);//座標を(0,1)に戻す
            this.enabled = false;
            scriptA.DisableGlow();
            CreateNextBlock();
        }
        if (BuildingStop == true)
        {
            if (myEvent != null)
            {
                //myEventに登録されている関数を実行
                myEvent.Invoke();
            }
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);//座標をその場にとどまる

            scriptA.DisableGlow();
            this.enabled = false;
            CreateNextBlock();
        }

        _fromMoveHorizonal += Time.deltaTime;

        if (_fromMoveHorizonal >= _restTime)
        {
            _pad = true;
            _fromMoveHorizonal = 0.0f;
        }
        if (Mathf.Ceil(_inputMove.y) == -1)
        {
            rb.velocity += new Vector2(0, Mathf.Abs(_inputMove.y) * -_downSpeed * 5);
        }


    }

    public void BuildingRotation(InputAction.CallbackContext context)
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
                _rotatePermission = false;
                break;
        }
        SoundManager.Instance.PlaySE(SESoundData.SE.MoveBuilding);
    }
    public void BuildingMove(InputAction.CallbackContext context)
    {
        _inputMove = context.ReadValue<Vector2>();
        //Debug.Log(_inputMove);
        SoundManager.Instance.PlaySE(SESoundData.SE.MoveBuilding);

    }

    static SpownBill cacheSpownBill;
    static SpownBill2P cachespownBill2P;
    /// <summary>
    /// 次のブロックの生成ロジック
    /// </summary>
    void CreateNextBlock()
    {
        switch (Player)
        {
            case PlayerNum.Player1:
                this.GetComponent<PlayerInput>().enabled = false;
                if (cacheSpownBill == null) cacheSpownBill = FindObjectOfType<SpownBill>();
                cacheSpownBill.NewBill();//新しいビルをリスポーン
                                         // このオブジェクトが無効化される時に、自身の座標を格納
                GameManager.Instance.SpownBill.BuildingPosition = transform.position;
                break;
            case PlayerNum.Player2:
                this.GetComponent<PlayerInput>().enabled = false;
                if (cachespownBill2P == null) cachespownBill2P = FindObjectOfType<SpownBill2P>();
                cachespownBill2P.NewBill2P();//新しいビルをリスポーン
                                             // このオブジェクトが無効化される時に、自身の座標を格納
                GameManager.Instance.SpownBill.BuildingPosition = transform.position;
                break;
        }
    }

    void ReturnToPastPosition(int frameCount)
    {
        int targetIndex = _pastPositions.Count /*- 1*/ - frameCount;
        if (targetIndex >= 0 && targetIndex < _pastPositions.Count)
        {
            float targetPosition = _pastPositions[targetIndex];
            this.transform.position = new Vector2(targetPosition, this.transform.position.y);
            rb.velocity = Vector2.zero;
        }

    }
    public void ChangeSpriteRandomly(SpriteRenderer renderer)
    {
        // スプライトの配列からランダムにスプライトを選択する
        Sprite randomSprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        // スプライトを変更する
        renderer.sprite = randomSprite;
    }

    /// <summary>
    /// ブロックの運動を無効化
    /// </summary>
    /// <param name="obj">対象のオブジェクト</param>
    public void FreezeAllConstrains(GameObject obj)
    {
        Rigidbody2D rb2D = obj.GetComponent<Rigidbody2D>();
        rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}