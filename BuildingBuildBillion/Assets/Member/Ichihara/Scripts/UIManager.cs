using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 建材オブジェクトのスプライト画像とそれに対応するサムネイル画像
/// </summary>
[System.Serializable]
public struct Thumbnail
{
    public Sprite BuildingSprite;
    public Sprite ThumbnailImage;
}

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // ゲーム中の UI を表示するキャンバス
    [SerializeField]
    private Canvas _gameUICanvas = null;
    // リザルト画面を表示するキャンバス
    [SerializeField]
    private Canvas _resultSceneCanvas = null;
    // 次建材表示の背景パネル
    [SerializeField]
    private Image _backGroundImage = null;
    // ゲーム開始までの待機時間を表示
    [SerializeField]
    private TextMeshProUGUI _waitingGameTimeText = null;
    public TextMeshProUGUI WaitingGameTimeText => _waitingGameTimeText;
    [System.NonSerialized]
    public bool IsStartedGameTime = false;

    #region ゲーム中のUI
    [Header("ゲーム中のUI")]
    // 制限時間を表示
    [SerializeField]
    private TextMeshProUGUI _timeText = null;
    // 次建材表示のサムネイル画像
    [SerializeField]
    private List<Thumbnail> _thumbnailImages = new List<Thumbnail>();
    // 次の建材表示 (1P)
    public Image Player1NextBuildingMaterial = null;
    // 次の建材表示 (2P)
    public Image Player2NextBuildingMaterial = null;
    [Space(3)]
    #endregion

    #region リザルトシーンの UI
    [Header("リザルトシーンのUI")]
    [SerializeField]
    private Sprite _youWon = null;
    public Sprite YouWon => _youWon;
    [SerializeField]
    private Sprite _youLost = null;
    public Sprite YouLost => _youLost;


    [SerializeField]
    private Sprite _youDraw = null;
    public Sprite YouDraw => _youDraw;
    [SerializeField]
    private Image _resultTtile = null;
    // 外部から書き換えるため。
    public Image Player1Result = null;
    public Image Player2Result = null;
    public GameObject DrawImage = null;
    [SerializeField]
    private GameObject _pleasePushToAImage = null;
  
    [Space(3)]
    #endregion

    #region アニメーション
    [SerializeField]
    private Animator _player1Anim = null;
    [SerializeField]
    private Animator _player2Anim = null;
    #endregion

    // 一度のみ実行するフラグ
    private bool _isDoneOnce = false;
    // キャンセル処理用のトークン
    private CancellationTokenSource _cts = new CancellationTokenSource();

    // Start is called before the first frame update
    void Start()
    {
        _resultTtile.color = Color.white;
        Player1Result.color = Color.clear;
        Player2Result.color = Color.clear;
        DrawImage.SetActive(false);
        _pleasePushToAImage.SetActive(false);

        // 次建材表示のバックグラウンドのカラー指定
        //_player1NextBack = GameObject.Find("Player1NextBack").GetComponent<Image>();
        // _player2NextBack = GameObject.Find("Player2NextBack").GetComponent<Image>();
        // ゲーム中に使用するキャンバスを非アクティブ化
        // 同時に、リザルトシーンのキャンバスを透明化
        _gameUICanvas.gameObject.SetActive(false);
        var resultSceneCanvasGroup = _resultSceneCanvas.GetComponent<CanvasGroup>();
        resultSceneCanvasGroup.alpha = 0.0f;
        _resultTtile.enabled = false;
        _waitingGameTimeText.color = new Color32(255, 177, 0, 255);
        _backGroundImage.color = new Color32(0, 0, 0, 128);

       
    }

    // Update is called once per frame
    async void Update()
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
            if (GameManager.Instance.CountDownGameTime < 0.0f)
            {
                _gameUICanvas.gameObject.SetActive(false);
                _waitingGameTimeText.color = Color.clear;
            }
            // 勝敗が確定したら、リザルトシーンのキャンバスを呼び出す
            // ついでにリザルトシーンの音声再生処理もここで呼び出す
            if (true == GameManager.Instance.IsPreviewedResult && false == _isDoneOnce)
            {
                _isDoneOnce = true;
                var resultSceneCanvasGroup = _resultSceneCanvas.GetComponent<CanvasGroup>();
                resultSceneCanvasGroup.alpha = 1.0f;
                await FadeInBackGroundImage(5.0f, 0.8f, _cts);
                SoundManager.Instance.PlayBGM(BGMSoundData.BGM.AnnouncementOfResult);
                _resultTtile.enabled = true;
                // 溜め時間
                await UniTask.DelayFrame(60 * 2, cancellationToken: _cts.Token);
                bool isDraw =JadgementBarController.Instance.Jadge();
                if(true == isDraw)
                {
                    Player1Result.color = Color.clear;
                    Player2Result.color = Color.clear;
                    DrawImage.SetActive(true);

                }
                else if(false == isDraw)
                {
                    Player1Result.color = Color.white;
                    Player2Result.color = Color.white;
                    DrawImage.SetActive(false);
                }
                PlayPlayer1ResultAnimation(Player1Result, _cts).Forget();
                PlayPlayer2ResultAnimation(Player2Result, _cts).Forget();
                await UniTask.DelayFrame(60 * 2, cancellationToken: _cts.Token);
                SoundManager.Instance.StopBGM();
                SoundManager.Instance.PlaySE(SESoundData.SE.Cheer);
                SoundManager.Instance.PlayBGM(BGMSoundData.BGM.ResultBGM);
                if (_pleasePushToAImage != null)
                {
                    _pleasePushToAImage.gameObject.SetActive(true);
                }
                GameManager.Instance.PlayerInput.enabled = true;
            }
        }
    }

    /// <summary>
    /// スプライトを取得し、次建材のサムネイル画像を表示する
    /// </summary>
    /// <param name="obj">表示するオブジェクトの種類</param>
    /// <returns></returns>
    public Sprite PreviewBuildingThumbnail(GameObject obj)
    {
        Sprite buildingSprite;
        try
        {
            buildingSprite = obj.GetComponent<SpriteRenderer>().sprite;
        }
        catch(MissingComponentException mce)
        {
            buildingSprite = obj.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        // 表示するサムネイル画像
        Sprite thumbnail = null;

        for (int i = 0; i < _thumbnailImages.Count; i++)
        {
            if (buildingSprite == _thumbnailImages[i].BuildingSprite)
            {
                thumbnail = _thumbnailImages[i].ThumbnailImage;
                break;
            }
        }
        return thumbnail;
    }

    /// <summary>
    /// ゲーム開始時の演出
    /// </summary>
    /// <param name="cts"></param>
    /// <returns></returns>
    public async UniTask StartGameEffect(CancellationTokenSource cts = default)
    {
        _waitingGameTimeText.fontSize = 300.0f;
        _waitingGameTimeText.text = "LET'S BUILD!!";
        SoundManager.Instance.PlaySE(SESoundData.SE.StartGame);
        await UniTask.Delay(1500, false, PlayerLoopTiming.Update, cts.Token);
        var waitingGameTimeTextParent = _waitingGameTimeText.GetComponentInParent<CanvasGroup>();
        waitingGameTimeTextParent.ignoreParentGroups = false;
        IsStartedGameTime = true;
        _gameUICanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// テキストを点滅
    /// </summary>
    /// <param name="tmp">フェードさせるテキスト</param>
    /// <param name="cts">キャンセル処理用のトークン</param>
    /// <returns></returns>
    private async UniTask FadeText(TextMeshProUGUI tmp, CancellationTokenSource cts = default)
    {
        bool isFaded = false;
        Color textColor = tmp.color;
        while (true)
        {
            if (false == isFaded)
            {
                textColor.a -= Time.deltaTime;
                tmp.color = new Color(textColor.r, textColor.g, textColor.b, textColor.a);
                if (textColor.a < 0.0f)
                {
                    textColor.a = 0.0f;
                    isFaded = true;
                }
            }
            else if (true == isFaded)
            {
                textColor.a += Time.deltaTime;
                tmp.color = new Color(textColor.r, textColor.g, textColor.b, textColor.a);
                if (textColor.a > 1.0f)
                {
                    textColor.a = 1.0f;
                    isFaded = false;
                }
            }
            await UniTask.Yield(cts.Token);
        }
    }

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    /// <param name="fadeOutTime">フェードアウトにかかる秒数</param>
    /// <param name="alphaRatio">開始時最終的な alpha 値の割合</param>
    /// <param name="cts">キャンセル処理用のトークン</param>
    /// <returns></returns>
    public async UniTask FadeOutBackGroundImage(float fadeOutTime, float alphaRatio = 0.5f, CancellationTokenSource cts = default)
    {
        bool isFaded = false;
        Color imageColor = _backGroundImage.color;
        while (false == isFaded)
        {
            imageColor.a -= Time.deltaTime * alphaRatio * (fadeOutTime * 0.1f);
            _backGroundImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, imageColor.a);
            if (imageColor.a < 0.0f)
            {
                imageColor.a = 0.0f;
                isFaded = true;
            }
            await UniTask.Yield(cts.Token);
        }
    }

    /// <summary>
    /// フェードイン処理
    /// </summary>
    /// <param name="fadeInTime">フェードインにかかる秒数</param>
    /// <param name="alphaRatio">開始時の alpha 値の割合</param>
    /// <param name="cts">キャンセル処理用のトークン</param>
    /// <returns></returns>
    private async UniTask FadeInBackGroundImage(float fadeInTime, float alphaRatio = 0.5f, CancellationTokenSource cts = default)
    {
        bool isFaded = false;
        Color imageColor = _backGroundImage.color;
        while (false == isFaded)
        {
            imageColor.a += Time.deltaTime * alphaRatio * (fadeInTime * 0.1f);
            _backGroundImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, imageColor.a);
            if (imageColor.a > alphaRatio)
            {
                imageColor.a = 0.3f;
                isFaded = true;
            }
            await UniTask.Yield(cts.Token);
        }
    }

    /// <summary>
    /// Player1 のリザルトアニメーション
    /// </summary>
    /// <param name="image">アニメーションするイメージ画像</param>
    /// <param name="cts">キャンセル処理用のトークン</param>
    /// <returns></returns>
    private async UniTask PlayPlayer1ResultAnimation(Image image, CancellationTokenSource cts = default)
    {
        if (image.sprite == _youWon)
        {
            try
            {
                await UniTask.DelayFrame(60 * 2, cancellationToken: cts.Token);
                _player1Anim.SetBool("WinPlayer", true);
            }
            catch (MissingReferenceException mre)
            {
                throw;
            }
        }
        else if (image.sprite == _youLost)
        {
            _player1Anim.SetBool("LosePlayer", true);
        }
    }

    /// <summary>
    /// Player2 のリザルトアニメーション
    /// </summary>
    /// <param name="image">アニメーションするイメージ画像</param>
    /// <returns>キャンセル処理用のトークン</returns>
    private async UniTask PlayPlayer2ResultAnimation(Image image, CancellationTokenSource cts = default)
    {
        if (image.sprite == _youWon)
        {
            try
            {
                await UniTask.DelayFrame(60 * 2, cancellationToken: cts.Token);
                _player2Anim.SetBool("WinPlayer", true);
            }
            catch (MissingReferenceException mre)
            {
                throw;
            }
        }
        else if (image.sprite == _youLost)
        {
            _player2Anim.SetBool("LosePlayer", true);
        }
    }
}
