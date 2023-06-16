using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // ゲーム中の UI を表示するキャンバス
    [SerializeField]
    private Canvas _gameUICanvas = null;
    // リザルト画面を表示するキャンバス
    [SerializeField]
    private Canvas _resultSceneCanvas = null;
    // 背景パネル
    [SerializeField]
    private Image _backGroundImage = null;

    // 各プレイヤーの名称
    [SerializeField]
    private string _player1Text = "Player 1";
    [SerializeField]
    private string _player2Text = "Player 2";

    [SerializeField]
    private TextMeshProUGUI _waitingGameTimeText = null;
    [System.NonSerialized]
    public bool IsStartedGameTime = false;

    #region ゲーム中のUI
    [Header("ゲーム中のUI")]
    // 制限時間を表示
    [SerializeField]
    private TextMeshProUGUI _timeText = null;
    // 次の建材表示 (1P)
    [SerializeField]
    private Image _player1NextBuildingMaterial = null;
    // 次の建材表示 (2P)
    [SerializeField]
    private Image _player2NextBuildingMaterial = null;
    // 次の建材表示の背景
    private Image _player1NextBack = null;
    private Image _player2NextBack = null;
    [Space(3)]
    #endregion

    #region リザルトシーンの UI
    [Header("リザルトシーンのUI")]
    [SerializeField]
    private string _youWon = "You won !!";
    public string YouWon => _youWon;
    [SerializeField]
    private string _youLost = "You lost ...";
    public string YouLost => _youLost;
    public TextMeshProUGUI Player1ResultText = null;
    public TextMeshProUGUI Player2ResultText = null;
    public TextMeshProUGUI DrawText = null;
    // 
    [SerializeField]
    private string _pleasePushToAText = "Please push to A";
    private TextMeshProUGUI _pleasePushToA = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // シーン上の TextMeshProUGUI を取得、初期化する
        var player1NameNext = GameObject.Find("Player1Text").GetComponent<TextMeshProUGUI>();
        player1NameNext.text = _player1Text;
        var player2NameNext = GameObject.Find("Player2Text").GetComponent<TextMeshProUGUI>();
        player2NameNext.text = _player2Text;
        // リザルトシーンキャンバスのテキストもこの段階で取得、初期化する
        var player1TextResult = GameObject.Find("Player1").GetComponent<TextMeshProUGUI>();
        player1TextResult.text = _player1Text;
        var player2TextResult = GameObject.Find("Player2").GetComponent<TextMeshProUGUI>();
        player2TextResult.text = _player2Text;
        _pleasePushToA = GameObject.Find("PleasePushToA").GetComponent<TextMeshProUGUI>();
        // 次建材表示のバックグラウンドのカラー指定
        _player1NextBack = GameObject.Find("Player1NextBack").GetComponent<Image>();
        _player1NextBack.color = new Color32(0, 0, 0, 85);
        _player2NextBack = GameObject.Find("Player2NextBack").GetComponent<Image>();
        _player2NextBack.color = new Color32(0, 0, 0, 85);
        // ゲーム中に使用するキャンバスを非アクティブ化
        // 同時に、リザルトシーンのキャンバスを透明化
        _gameUICanvas.gameObject.SetActive(false);
        var resultSceneCanvasGroup = _resultSceneCanvas.GetComponent<CanvasGroup>();
        resultSceneCanvasGroup.alpha = 0.0f;
        // 背景を黒の半透明にしておく
        _waitingGameTimeText.color = Color.black;
        _backGroundImage.color = new Color32(0, 0, 0, 128);
        // DrawText の中身を空文字で初期化する
        DrawText.text = "";
    }

    private bool _isDoneOnce = false;

    // Update is called once per frame
    void Update()
    {
        // ゲーム開始まで待機
        if (GameManager.Instance.WaitingGameTime > 0.0f)
        {
            _waitingGameTimeText.text = string.Format("{0:#}", GameManager.Instance.WaitingGameTime);
        }
        if (false == IsStartedGameTime) { return; }
        else
        {
            _timeText.text = string.Format("{0:#}", GameManager.Instance.CountDownGameTime);
            if (GameManager.Instance.CountDownGameTime > 0.0f)
            {
                _player1NextBuildingMaterial.sprite = PreviewBuildingSprite(GameManager.Instance.Obj);
                _player2NextBuildingMaterial.sprite = PreviewBuildingSprite(GameManager.Instance.Obj2);
            }
            // 勝敗が確定したら、リザルトシーンを呼び出す。
            if (true == GameManager.Instance.IsPreviewedResult)
            {
                _gameUICanvas.gameObject.SetActive(false);
                _waitingGameTimeText.color = Color.clear;
                var resultSceneCanvasGroup = _resultSceneCanvas.GetComponent<CanvasGroup>();
                resultSceneCanvasGroup.alpha = 1.0f;
                // 表示テキストを初期化
                _pleasePushToA.text = _pleasePushToAText;
                if(false == _isDoneOnce)
                {
                    JadgementBarController.Instance.Jadge();
                    Fade().Forget();
                    _isDoneOnce = true;
                }
            }
        }
    }

    /// <summary>
    /// スプライトを取得
    /// </summary>
    /// <param name="obj">表示するオブジェクトの種類</param>
    /// <param name="sprite">表示するスプライト</param>
    /// <returns></returns>
    private Sprite PreviewBuildingSprite(in GameObject obj, Sprite sprite = default)
    {
        sprite = obj.GetComponent<SpriteRenderer>().sprite;
        return sprite;
    }

    /// <summary>
    /// ゲーム開始時の演出
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask StartGameEffect(CancellationToken token = default)
    {
        _waitingGameTimeText.fontSize = 300.0f;
        _waitingGameTimeText.text = "Let's build!!!";
        // 要変更
        await UniTask.Delay(1500, false, PlayerLoopTiming.Update, token);
        var waitingGameTimeTextParent = _waitingGameTimeText.GetComponentInParent<CanvasGroup>();
        waitingGameTimeTextParent.ignoreParentGroups = false;
        IsStartedGameTime = true;
        _gameUICanvas.gameObject.SetActive(true);
    }

    private async UniTask Fade(bool isFaded = false, CancellationToken token = default)
    {
        Color textColor = _pleasePushToA.color;
        while(true)
        {
            if (false == isFaded)
            {
                textColor.a -= Time.deltaTime;
                _pleasePushToA.color = new Color(textColor.r, textColor.g, textColor.b, textColor.a);
                if (textColor.a < 0.0f)
                {
                    textColor.a = 0.0f;
                    isFaded= true;
                }
            }
            else if (true == isFaded)
            {
                textColor.a += Time.deltaTime;
                _pleasePushToA.color = new Color(textColor.r, textColor.g, textColor.b, textColor.a);
                if (textColor.a > 1.0f)
                {
                    textColor.a = 1.0f;
                    isFaded = false;
                }
            }
            await UniTask.Yield(token);
        }
    }
}
