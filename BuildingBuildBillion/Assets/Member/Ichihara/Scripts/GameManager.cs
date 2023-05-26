using Cysharp.Threading.Tasks;
using System.Threading;
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

    [Header("建材オブジェクトがスポーンする座標")]
    [SerializeField]
    private GameObject _buildSpawnPoint1 = null;
    private Vector2 _defaultBuildSpawnPoint1 = Vector2.zero;
    [SerializeField]
    private GameObject _buildSpawnPoint2 = null;
    private Vector2 _defaultBuildSpawnPoint2 = Vector2.zero;

    public SpownBill SpownBill => _buildSpawnPoint1.GetComponent<SpownBill>();
    public SpownBill2P SpownBill2P => _buildSpawnPoint2.GetComponent<SpownBill2P>();

    [System.NonSerialized]
    public bool IsEndGame = false;

    // Start is called before the first frame update
    void Start()
    {
        if (_buildSpawnPoint1 == null || _buildSpawnPoint2 == null)
        {
            Debug.LogError("アタッチされてねーよ！！");
        }
        _countDownTime = _setTime;
        // (0, 0)がスクリーンの中央である為、与えた値の半分にする
        _buildingHeightAndScreenRatio *= 1.0f / 2.0f;
        // _buildSpawnPoint の初期値を代入
        _defaultBuildSpawnPoint1 = _buildSpawnPoint1.transform.position;
        _defaultBuildSpawnPoint2 = _buildSpawnPoint2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CountDown();
        // CalucrateCameraMovement を開始する条件
        // 要変更
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
        if (_countDownTime < 0.0f)
        {
            // カウントダウンが終了したら操作中のオブジェクトを破棄する
            Destroy(SpownBill.Obj);
            Destroy(SpownBill2P.Obj);
            return;
        }
        _countDownTime -= Time.deltaTime;
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
        await UniTask.WhenAll(SearchRigidbody2D(bill, token),
                              SearchRigidbody2D(bill2, token));
    }

    /// <summary>
    /// 画面にあるすべての建材の Rigidbody2D を検索、取得
    /// </summary>
    /// <param name="objects">建材オブジェクトの配列</param>
    /// <param name="token">キャンセル処理のトークン</param>
    /// <returns></returns>
    private async UniTask SearchRigidbody2D(GameObject[] objects, CancellationToken token = default)
    {
        foreach (GameObject obj in objects)
        {
            var component1 = obj.GetComponent<billcontroller>();
            var component2 = obj.GetComponent<billcontroller2P>();
            if (component1 != null)
            {
                billcontroller billController = obj.GetComponent<billcontroller>();
                billController.FreezeAll(obj);
            }
            else if (component2 != null)
            {
                billcontroller2P billController2P = obj.GetComponent<billcontroller2P>();
                billController2P.FreezeAll(obj);
            }
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
        }
    }

    /// <summary>
    /// 建材がスポーンするポイントを移動させる
    /// </summary>
    /// <param name="yAxis">移動方向のベクトル</param>
    public void MoveBuildSpawnPoint(bool yAxis, float speed)
    {
        if (yAxis == true)
        {
            _buildSpawnPoint1.transform.position += new Vector3(0.0f
                                                              , Vector3.up.y * speed * Time.deltaTime
                                                              , 0.0f);
            _buildSpawnPoint2.transform.position += new Vector3(0.0f
                                                              , Vector3.up.y * speed * Time.deltaTime
                                                              , 0.0f);
        }
        else if (yAxis == false)
        {
            _buildSpawnPoint1.transform.position += new Vector3(0.0f
                                                              , Vector3.down.y * speed * Time.deltaTime
                                                              , 0.0f);
            _buildSpawnPoint2.transform.position += new Vector3(0.0f
                                                              , Vector3.down.y * speed * Time.deltaTime
                                                              , 0.0f);
        }
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

}
