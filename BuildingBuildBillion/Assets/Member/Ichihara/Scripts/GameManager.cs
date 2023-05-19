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

    // Start is called before the first frame update
    void Start()
    {
        _countDownTime = _setTime;
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
