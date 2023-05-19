using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // コントローラに引き渡す用の emun 変数
    public enum BuildingMaterialStatus
    {
        Normal1,
        Normal2,
        Cannon,
        ConveyorBelt,
        Crane,
    }

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
        // CalucrateCameraMovement を開始する条件
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CameraControllerTest.Instance.CallCalucrateCameraMovement();
        }
    }

    /// <summary>
    /// カウントダウン処理
    /// </summary>
    private void CountDown()
    {
        _countDownTime -= Time.deltaTime;
    }

    /// <summary>
    /// カメラのズーム、移動に連動して JadgementBarFallPoint を Y 軸方向に移動させる
    /// </summary>
    /// <param name="yAxis">カメラのズームの移動方向</param>
    public void MoveJadgementBarFallPoint(bool yAxis)
    {
        if (yAxis == true)
        {
            _jadgementBarFallPoint.transform.position += Vector3.up * _jadgementBarFallPointHeight * Time.deltaTime;
        }
        else if (yAxis == false)
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
