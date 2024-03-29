﻿using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region 制限時間
    [Header("制限時間")]
    // ゲームの制限時間
    [SerializeField]
    private float _setGameTime = 0.0f;
    private float _countDownGameTime = 0.0f;
    public float CountDownGameTime => _countDownGameTime;
    // ゲーム開始まで待機する時間
    [SerializeField]
    private float _setWaitingGameTime = 0.0f;
    private float _waitingGameTime = 0.0f;
    public float WaitingGameTime => _waitingGameTime;
    [Space(3)]
    #endregion

    // 画面内に収めるビルの高さと画面の割合
    [SerializeField, Range(0.0f, 1.0f)]
    private float _buildingHeightAndScreenRatio = 0.8f;
    public float BuildingHeightAndScreenRatio => _buildingHeightAndScreenRatio;

    #region 参照
    [Header("建材オブジェクトがスポーンする座標")]
    [SerializeField]
    private GameObject _buildSpawnPoint1 = null;
    public SpownBill SpownBill => _buildSpawnPoint1.GetComponent<SpownBill>();
    private Vector2 _defaultBuildSpawnPoint1 = Vector2.zero;
    [SerializeField]
    private GameObject _buildSpawnPoint2 = null;
    public SpownBill2P SpownBill2P => _buildSpawnPoint2.GetComponent<SpownBill2P>();
    private Vector2 _defaultBuildSpawnPoint2 = Vector2.zero;
    // 1P が操作しているオブジェクト
    [System.NonSerialized]
    public GameObject Obj = null;
    // 2P が操作しているオブジェクト
    [System.NonSerialized]
    public GameObject Obj2 = null;
    [System.NonSerialized]
    public PlayerInput PlayerInput = null;
    #endregion

    // ゲーム終了判定
    [System.NonSerialized]
    public bool IsEndedGame = false;
    // リザルト画面を表示するフラグ
    [System.NonSerialized]
    public bool IsPreviewedResult = false;

    // Start is called before the first frame update
    async void Start()
    {
        if (_buildSpawnPoint1 == null || _buildSpawnPoint2 == null)
        {
            Debug.LogError("アタッチされてねーよ！！");
        }
        IsEndedGame = false;
        IsPreviewedResult = false;
        _countDownGameTime = _setGameTime;
        _waitingGameTime = _setWaitingGameTime;
        // (0, 0)がスクリーンの中央である為、与えた値の半分にする
        _buildingHeightAndScreenRatio *= 1.0f / 2.0f;
        // _buildSpawnPoint の初期値を代入
        _defaultBuildSpawnPoint1 = _buildSpawnPoint1.transform.position;
        _defaultBuildSpawnPoint2 = _buildSpawnPoint2.transform.position;
        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.enabled = false;
        await CountDownToStartTheGame();
    }

    private void Update()
    {
        QuitApplication();
    }

    /// <summary>
    /// ゲーム時間のカウントダウン処理
    /// </summary>
    private void CountDown()
    {
        if (_countDownGameTime < 0.0f) { return; }
        _countDownGameTime -= Time.deltaTime;
    }

    /// <summary>
    /// ゲームを開始するまでのカウントダウン処理
    /// </summary>
    /// <param name="token">キャンセル処理用のトークン</param>
    /// <returns></returns>
    private async UniTask CountDownToStartTheGame(CancellationToken token = default)
    {
        bool countThree = false;
        bool countTwo = false;
        bool countOne = false;
        while (_waitingGameTime > 0.0f)
        {
            if (countThree == false && UIManager.Instance.WaitingGameTimeText.text == "3")
            {
                SoundManager.Instance.PlaySE(SESoundData.SE.CountDown);
                countThree = true;
            }
            else if (countTwo == false && UIManager.Instance.WaitingGameTimeText.text == "2")
            {
                SoundManager.Instance.PlaySE(SESoundData.SE.CountDown);
                countTwo = true;
            }
            else if (countOne == false && UIManager.Instance.WaitingGameTimeText.text == "1")
            {
                SoundManager.Instance.PlaySE(SESoundData.SE.CountDown);
                countOne = true;
            }
            _waitingGameTime -= Time.deltaTime;
            UIManager.Instance.FadeOutImage(3.0f).Forget();
            await UniTask.Yield(token);
        }
        // ここに開始時の演出を加える
        await UIManager.Instance.StartGameEffect();
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.GameBGM);
        SpownBill.NewBill();
        SpownBill2P.NewBill2P();
        while (_countDownGameTime > 0.0f)
        {
            CountDown();
            await UniTask.Yield(token);
        }
        //
        SoundManager.Instance.StopBGM();
    }

    /// <summary>
    /// 画面にあるすべての建材の Rigidbody2D を検索、取得
    /// </summary>
    /// <param name="objects">フィールドに存在する建材オブジェクトを格納する配列</param>
    /// <param name="token">キャンセル処理のトークン</param>
    /// <returns></returns>
    private async UniTask SearchController(GameObject[] objects, CancellationToken token = default)
    {
        foreach (GameObject obj in objects)
        {
            if (obj == null) { break; }
            var component1 = obj.GetComponent<NewBuildingcon>();
            var component2 = obj.GetComponent<NewBuildingcon>();
            if (component1 != null)
            {
                NewBuildingcon newBuildingCon = obj.GetComponent<NewBuildingcon>();
                newBuildingCon.FreezeAllConstrains(obj);
            }
            else if (component2 != null)
            {
                NewBuildingcon newBuildingcon = obj.GetComponent<NewBuildingcon>();
                newBuildingcon.FreezeAllConstrains(obj);
            }
            await UniTask.Yield(token);
        }
    }

    /// <summary>
    /// ゲーム終了時の処理
    /// </summary>
    /// <param name="token">キャンセル処理のトークン</param>
    /// <returns></returns>
    public async UniTask EndGame(CancellationToken token = default)
    {
        var bill = GameObject.FindGameObjectsWithTag("Bill");
        var bill2 = GameObject.FindGameObjectsWithTag("Bill2");
        await UniTask.WhenAll(SearchController(bill, token),
                              SearchController(bill2, token));
    }

    /// <summary>
    /// 建材がスポーンするポイントを移動させる
    /// </summary>
    public void MoveBuildingSpawnPoint(float zoom)
    {
        _buildSpawnPoint1.transform.position += Vector3.up * zoom;
        _buildSpawnPoint2.transform.position += Vector3.up * zoom;

        // 各スポーンポイントの移動限界値を設定
        _buildSpawnPoint1.transform.position = new Vector3(_buildSpawnPoint1.transform.position.x
                                                         , Mathf.Clamp(_buildSpawnPoint1.transform.position.y
                                                                     , _defaultBuildSpawnPoint1.y
                                                                     , Screen.height * 1.5f)
                                                         , 0.0f);
        _buildSpawnPoint2.transform.position = new Vector3(_buildSpawnPoint2.transform.position.x
                                                         , Mathf.Clamp(_buildSpawnPoint2.transform.position.y
                                                                     , _defaultBuildSpawnPoint2.y
                                                                     , Screen.height * 1.5f)
                                                         , 0.0f);
    }

    public void ChangeTitleScene(InputAction.CallbackContext context)
    {
        if(IsPreviewedResult == true && context.phase == InputActionPhase.Canceled)
        {
            SceneMove.instance.TitleMove();
        }
    }

    /// <summary>
    /// アプリ終了
    /// </summary>
    private void QuitApplication()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneMove.instance.TitleMove();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();        
#endif
        }
    }
}
