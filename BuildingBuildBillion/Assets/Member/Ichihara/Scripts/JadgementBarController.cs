using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class JadgementBarController : SingletonMonoBehaviour<JadgementBarController>
{
    [Header("勝敗判定")]
    // 勝敗判定バー
    [SerializeField]
    private GameObject _jadgementBar = null;
    private Rigidbody2D _rb2D = null;
    // 画面トップから JadgementBarFallPoint をどのくらい離すかを決める
    [SerializeField]
    private float _jadgementBarFallPointHeight = 0.0f;
    // アタッチされているオブジェクトの初期座標
    private Vector3 _defaultJadgementBarFallPosition = Vector3.zero;
    // JadgementBar に接触したオブジェクトを格納
    [System.NonSerialized]
    public List<GameObject> Objects = new List<GameObject>();
    [Space(3)]
    // 重力の大きさを調整する
    [SerializeField, Range(0.0f, 10.0f)]
    private float _gravityScaleParameter = 5.0f;

    private CancellationTokenSource cts = new CancellationTokenSource();

    // Start is called before the first frame update
    void Start()
    {
        // JadgementBar クラスが Rigidbody と Collider2D を RequireComponent している
        _jadgementBar.AddComponent<JadgementBar>();
        // ゲーム中は必要ないので enabled を false にする
        _jadgementBar.GetComponent<SpriteRenderer>().enabled = false;
        _jadgementBar.GetComponent<Collider2D>().enabled = false;
        _rb2D = _jadgementBar.GetComponent<Rigidbody2D>();
        _rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb2D.gravityScale = 0.0f;
        // このスクリプトアタッチされているオブジェクトの初期座標で初期化
        _defaultJadgementBarFallPosition = Vector3.up * (Screen.height / 2.0f + _jadgementBarFallPointHeight) + Vector3.right * 60.0f;
        transform.position = _defaultJadgementBarFallPosition;
    }

    private async void FixedUpdate()
    {
        // 変更の可能性有
        if (GameManager.Instance.CountDownGameTime < 0.0f)
        {
            if (false == GameManager.Instance.IsEndedGame)
            {
                GameManager.Instance.IsEndedGame = true;
                // カウントダウンが終了したら操作中のオブジェクトを破棄する
                Destroy(GameManager.Instance.Obj);
                Destroy(GameManager.Instance.Obj2);
                await GameManager.Instance.EndGame(cts);
                // 勝敗判定に必要なので enabled を true にする
                _jadgementBar.GetComponent<SpriteRenderer>().enabled = true;
                _jadgementBar.GetComponent<Collider2D>().enabled = true;
            }
            _rb2D.gravityScale = _gravityScaleParameter;
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
                                        , Mathf.Clamp(transform.position.y
                                                    , _defaultJadgementBarFallPosition.y
                                                    , Screen.height * 1.5f + _jadgementBarFallPointHeight)
                                        , 0.0f);
    }

    /// <summary>
    /// 勝敗判定
    /// </summary>
    public bool Jadge()
    {
        // 判定バーに触れたオブジェクトの座標を取得し、
        // それらの x 座標の位置で勝敗を決める
        Vector2[] buildingPosition = new Vector2[Objects.Count];
        int rightOfCenter = 0;
        int leftOfCenter = 0;

        for (int i = 0; i < buildingPosition.Length; i++)
        {
            buildingPosition[i] = Objects[i].transform.position;
            if (buildingPosition[i].x < 0.0f)
            {
                rightOfCenter++;
            }
            else if (buildingPosition[i].x > 0.0f)
            {
                leftOfCenter++;
            }
        }

        bool isPreviewDraw = false;
        // 引き分け処理
        if (Objects.Count >= 2 || rightOfCenter == leftOfCenter)
        {
            UIManager.Instance.DrawImage.sprite
                 = UIManager.Instance.YouDraw;
            isPreviewDraw = true;
            return isPreviewDraw;
        }
        else if (rightOfCenter > leftOfCenter)
        {
            UIManager.Instance.Player1Result.sprite
                = UIManager.Instance.YouWon;
            UIManager.Instance.Player2Result.sprite
                = UIManager.Instance.YouLost;
        }
        else if (rightOfCenter < leftOfCenter)
        {
            UIManager.Instance.Player1Result.sprite
                = UIManager.Instance.YouLost;
            UIManager.Instance.Player2Result.sprite
                = UIManager.Instance.YouWon;
        }
        return isPreviewDraw;
    }
}
