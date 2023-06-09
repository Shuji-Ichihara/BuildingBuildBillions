using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private Canvas _resultBackGroundCanvas = null;

    [SerializeField]
    private string _player1Text = "Player 1";
    [SerializeField]
    private string _player2Text = "Player 2";

    #region ゲーム中のUI
    [Header("ゲーム中のUI")]
    // 制限時間を表示
    [SerializeField]
    private TextMeshProUGUI _timeText = null;
    // プレイヤー番号を表示(ゲーム中)
    private TextMeshProUGUI _player1Next = null;
    private TextMeshProUGUI _player2Next = null;
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
    [SerializeField]
    private string _youWon = "You won !!";
    public string YouWon => _youWon;
    [SerializeField]
    private string _youLost = "You lost ...";
    public string YouLost => _youLost;
    public TextMeshProUGUI Player1ResultText = null;
    public TextMeshProUGUI Player2ResultText = null;
    // 
    [SerializeField]
    private string _pleasePushToAText = "Please push to A";
    private TextMeshProUGUI _pleasePushToA = null;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // シーン上の TextMeshProUGUI を取得
        _player1Next = GameObject.Find("Player1Next").GetComponent<TextMeshProUGUI>();
        _player2Next = GameObject.Find("Player2Next").GetComponent<TextMeshProUGUI>();
        _pleasePushToA = GameObject.Find("PleasePushToA").GetComponent<TextMeshProUGUI>();
        // 各キャンバスのアクティブ、非アクティブを操作
        _gameUICanvas.gameObject.SetActive(true);
        _resultSceneCanvas.gameObject.SetActive(false);
        // 次建材表示のバックグラウンドのカラー指定
        _player1NextBack = GameObject.Find("Player1NextBack").GetComponent<Image>();
        _player1NextBack.color = new Color32(0, 0, 0, 85);
        _player2NextBack = GameObject.Find("Player2NextBack").GetComponent<Image>();
        _player2NextBack.color = new Color32(0, 0, 0, 85);
    }

    // Update is called once per frame
    void Update()
    {
        _timeText.text = string.Format("{0:#}", GameManager.Instance.CountDownTime);
        if(GameManager.Instance.CountDownTime > 0.0f)
        {
            _player1NextBuildingMaterial.sprite = PreviewBuildingMaterial1P(GameManager.Instance.Obj);
            _player2NextBuildingMaterial.sprite = PreviewBuildingMaterial2P(GameManager.Instance.Obj2);
        }
        // 勝敗が確定したら、リザルトシーンを呼び出す。
        if (true == GameManager.Instance.IsPreviewResult
            && true == _gameUICanvas.gameObject.activeSelf
            && false == _resultSceneCanvas.gameObject.activeSelf)
        {
            _gameUICanvas.gameObject.SetActive(false);
            _resultSceneCanvas.gameObject.SetActive(true);
            // 表示テキストを初期化
            _player1Next.text = _player1Text;
            _player2Next.text = _player2Text;
            _pleasePushToA.text = _pleasePushToAText;
        }
    }

    /// <summary>
    /// Obj のスプライトを取得
    /// </summary>
    /// <param name="obj">表示するオブジェクトの種類</param>
    /// <param name="sprite">表示するスプライト</param>
    /// <returns></returns>
    private Sprite PreviewBuildingMaterial1P(in GameObject obj, Sprite sprite = default )
    {
        sprite = obj.GetComponent<SpriteRenderer>().sprite;
        return sprite;
    }

    /// <summary>
    /// Obj2 のスプライトを取得
    /// </summary>
    /// <param name="obj">表示するオブジェクトの種類</param>
    /// <param name="sprite">表示するスプライト</param>
    /// <returns></returns>
    private Sprite PreviewBuildingMaterial2P(in GameObject obj, Sprite sprite = default)
    {
        sprite = obj.GetComponent<SpriteRenderer>().sprite;
        return sprite;
    }
}
