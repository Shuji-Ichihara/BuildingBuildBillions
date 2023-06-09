using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class InputTest : MonoBehaviour
{
    public enum PlayerNum
    {
        Player1 =0,
        Player2 =1
    }

    public PlayerNum jittai;
    // ボタンの押下状態
    private bool _isLeftPressed = false;    //LBが押されたか？
    private bool _isRightPressed = false;   //RBが押されたか？
    private bool _isHozirontalPressed = false;//左右キーが押されたか？
    private Vector2 _inputMove = Vector2.zero; //ブロック移動の数値取得
    private float restTime = 0; //左右キーの単押しするための制限

    private void Update()
    {
        switch (jittai)
        {
            case PlayerNum.Player1:
                //プレイヤー１の移動処理
                break;
            case PlayerNum.Player2:
                //プレイヤー２の移動処理
                break;
        }
        restTime += Time.deltaTime;
        if (_isLeftPressed)
        {
            Debug.Log("hidarinaga");    //左押し処理
        }
        if (_isRightPressed)
        {
            Debug.Log("miginaga");    //右押し処理
        }
        //左右単押し
        if (_inputMove.x != 0 && restTime >= 0.25f && _isHozirontalPressed == true)
        {
            _isHozirontalPressed = false;
            //_inputMove.x,this.tra.pos.y,0;
            Debug.Log(_inputMove.x);  //ブロック左右移動処理
            restTime = 0;
        }
        //下長押し
        Debug.Log(_inputMove.y); //下押しの処理
    }

    public void Rotate(InputAction.CallbackContext context) //キー入力を受け付けるための関数
    {
        Debug.Log(context.phase);
        var y = context.control.name;   //押されたキーの名前を取得

        switch (context.phase)  //入力状態で判断
        {
            case InputActionPhase.Performed:    //入力されていたら

                if (y == "leftShoulder")    //入力されていて名前が条件式にあっていたらboolをtrueにする
                {
                    _isLeftPressed = true;
                }
                if (y == "rightShoulder")//入力されていて名前が条件式にあっていたらboolをtrueにする
                {
                    Debug.Log("migi");
                    _isRightPressed = true;
                }
                break;
            case InputActionPhase.Canceled:     //入力が終了したらboolをすべてfalseにする
                _isLeftPressed = false;
                _isRightPressed = false;
                break;
        }
    }

    // 左右移動
    public void OnHold(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
        _isHozirontalPressed = true;
    }
}

