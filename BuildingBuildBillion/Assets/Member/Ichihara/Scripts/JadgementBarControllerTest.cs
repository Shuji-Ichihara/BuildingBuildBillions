using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadgementBarControllerTest : SingletonMonoBehaviour<JadgementBarControllerTest>
{
    [Header("勝敗判定")]
    // 勝敗判定バー
    [SerializeField]
    private GameObject _jadgementBar = null;
    private Rigidbody2D _rb2D = null;
    // 画面トップから _jadgementBarFallPoint をどのくらい離すかを決める
    [SerializeField]
    private float _jadgementBarFallPointHeight = 0.0f;
    // アタッチされているオブジェクトの初期座標
    private Vector3 _jadgementBarFallPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // JadgementBar クラスが Rigidbody を RequireComponent している
        _jadgementBar.AddComponent<JadgementBar>();
        _rb2D = _jadgementBar.GetComponent<Rigidbody2D>();
        _rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb2D.gravityScale = 0.0f;
        // このスクリプトアタッチされているオブジェクトの初期座標で初期化
        _jadgementBarFallPosition = Vector3.up * (Screen.height / 2.0f + _jadgementBarFallPointHeight);
        transform.position = _jadgementBarFallPosition;
    }

    private void FixedUpdate()
    {
        // 変更の可能性有
        if(GameManager.Instance.CountDownTime < 0.0f)
        {
            _rb2D.gravityScale = 1.0f;
        }
    }


    /// <summary>
    /// カメラのズーム、移動に連動して JadgementBarFallPoint を Y 軸方向に移動させる
    /// </summary>
    /// <param name="yAxis">カメラの Y 軸の移動方向</param>
    public void MoveJadgementBarFallPoint(bool yAxis)
    {
        if (yAxis == true)
        {
            transform.position += Vector3.up * _jadgementBarFallPointHeight * Time.deltaTime;
        }
        else if (yAxis == false)
        {
            transform.position += Vector3.down * _jadgementBarFallPointHeight * Time.deltaTime;
        }
        transform.position = new Vector3(0.0f
                                        ,Mathf.Clamp(transform.position.y
                                                    ,_jadgementBarFallPosition.y
                                                    ,Screen.height * 1.5f + _jadgementBarFallPointHeight)
                                        ,0.0f);
    }

    /// <summary>
    /// 勝敗判定
    /// </summary>
    /// <param name="vec2">接触したオブジェクトの 2D 座標</param>
    public void Jadge(Vector2 vec2)
    {
        if (vec2.x < 0.0f)
        {
            Debug.Log($"Player1 Win!!");
        }
        else if (vec2.x > 0.0f)
        {
            Debug.Log($"Player2 Win!!");
        }
        // 接触したオブジェクトの x 座標が 0 の場合(後で取り掛かる)
        else
        {
            Debug.Log($"Draw");
        }
    }
}
