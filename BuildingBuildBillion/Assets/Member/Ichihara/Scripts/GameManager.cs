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

    // 画面内に収めるビルの高さと画面の割合
    [SerializeField, Range(0.0f, 1.0f)]
    private float _buildingHeightAndScreenRatio = 0.8f;
    public float BuildingHeightAndScreenRatio => _buildingHeightAndScreenRatio;

    // Start is called before the first frame update
    void Start()
    {
        _countDownTime = _setTime;
        // (0, 0)がスクリーンの中央である為、与えた値の半分にする
        _buildingHeightAndScreenRatio *= 1.0f / 2.0f;
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
        if (_countDownTime < 0.0f) { return; }
        _countDownTime -= Time.deltaTime;
    }

}
