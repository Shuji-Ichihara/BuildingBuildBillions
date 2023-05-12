using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("制限時間")]
    [SerializeField]
    private float _setTime = 0.0f;
    private float _countDownTime = 0.0f;
    public float CountDownTime => _countDownTime;
    [Space(3)]

    [Header("勝利判定")]
    [SerializeField]
    private GameObject _jadgementBarFallPoint = null;
    // 画面トップから _jadgementBarFallPoint をどのくらい離すかを決める
    [SerializeField]
    private float _jadgementBarFallPointHeight = 0.0f;
    // _jadgementBarFallPoint の初期座標
    private Vector3 _jadgementBarFallPosition = Vector3.zero;   

    // Start is called before the first frame update
    void Start()
    {
        _countDownTime = _setTime;
        _jadgementBarFallPosition = Vector3.up * (Screen.height / 2.0f + _jadgementBarFallPointHeight);
        _jadgementBarFallPoint.transform.position = _jadgementBarFallPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CountDown();
    }

    /// <summary>
    /// カウントダウン処理
    /// </summary>
    private void CountDown()
    {
        _countDownTime -= Time.deltaTime;
    }

    /// <summary>
    /// カメラのズーム、移動に連動して JadgementBarFallPoint の Y 座標を移動させる
    /// </summary>
    /// <param name="zoom">カメラのズームの移動量</param>
    public void MoveJadgementBarFallPoint(float zoom)
    {
        if (zoom > 0.0f)
        {
            _jadgementBarFallPoint.transform.position += Vector3.up * _jadgementBarFallPointHeight * Time.deltaTime;
        }
        else if (zoom < 0.0f)
        {
            _jadgementBarFallPoint.transform.position += Vector3.down * _jadgementBarFallPointHeight * Time.deltaTime;
        }
        _jadgementBarFallPoint.transform.position = new Vector3(0.0f
                                                              , Mathf.Clamp(_jadgementBarFallPoint.transform.position.y
                                                                          , _jadgementBarFallPosition.y
                                                                          , Screen.height * 1.5f + _jadgementBarFallPointHeight)
                                                              , 0.0f);
    }
}
