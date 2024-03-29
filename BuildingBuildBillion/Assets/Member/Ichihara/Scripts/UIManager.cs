﻿using System.Collections;
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
    // 外部から書き換えるため。
    public TextMeshProUGUI Player1ResultText = null;
    public TextMeshProUGUI Player2ResultText = null;
    public TextMeshProUGUI DrawText = null;
    [SerializeField]
    private string _pleasePushToAText = "Please push to A";
    private TextMeshProUGUI _pleasePushToA = null;
    [Space(3)]
    #endregion
    // 一度のみ実行するフラグ
    private bool _isDoneOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        Player1ResultText.text = "";
        Player2ResultText.text = "";
        _pleasePushToA = GameObject.Find("PleasePushToA").GetComponent<TextMeshProUGUI>();
        _pleasePushToA.text = "";
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
        _waitingGameTimeText.color = Color.yellow;
        _backGroundImage.color = new Color32(0, 0, 0, 128);
        // DrawText の中身を空文字で初期化する
        DrawText.text = "";
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
                SoundManager.Instance.PlayBGM(BGMSoundData.BGM.AnnouncementOfResult);
                await FadeInImage(5.0f, 0.8f);
                // AnnouncementOfResult を停止
                SoundManager.Instance.PlaySE(SESoundData.SE.Cheer);
                SoundManager.Instance.PlayBGM(BGMSoundData.BGM.ResultBGM);
                JadgementBarController.Instance.Jadge();
                _pleasePushToA = GameObject.Find("PleasePushToA").GetComponent<TextMeshProUGUI>();
                _pleasePushToA.text = _pleasePushToAText;
                FadeText().Forget();
                GameManager.Instance.PlayerInput.enabled = true;
            }
        }
    }

    /// <summary>
    /// スプライトを取得し、次建材のサムネイル画像を表示する
    /// </summary>
    /// <param name="obj">表示するオブジェクトの種類</param>
    /// <returns></returns>
    public Sprite PreviewBuildingSprite(in GameObject obj)
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
        Sprite thumbnail = null;

        for (int i = 0; i < _thumbnailImages.Count; i++)
        {
            if (buildingSprite == _thumbnailImages[i].BuildingSprite || buildingSprite == _thumbnailImages[i].BuildingSprite)
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
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask StartGameEffect(CancellationToken token = default)
    {
        _waitingGameTimeText.fontSize = 300.0f;
        _waitingGameTimeText.text = "Let's build!!!";
        SoundManager.Instance.PlaySE(SESoundData.SE.StartGame);
        await UniTask.Delay(1500, false, PlayerLoopTiming.Update, token);
        var waitingGameTimeTextParent = _waitingGameTimeText.GetComponentInParent<CanvasGroup>();
        waitingGameTimeTextParent.ignoreParentGroups = false;
        IsStartedGameTime = true;
        _gameUICanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// テキストを点滅
    /// </summary>
    /// <param name="isFaded">alpha の増減切り替え</param>
    /// <param name="token">キャンセル処理用のトークン</param>
    /// <returns></returns>
    private async UniTask FadeText(bool isFaded = false, CancellationToken token = default)
    {
        Color textColor = _pleasePushToA.color;
        while (true)
        {
            if (false == isFaded)
            {
                textColor.a -= Time.deltaTime;
                _pleasePushToA.color = new Color(textColor.r, textColor.g, textColor.b, textColor.a);
                if (textColor.a < 0.0f)
                {
                    textColor.a = 0.0f;
                    isFaded = true;
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

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    /// <param name="fadeOutTime">フェードアウトにかかる秒数</param>
    /// <param name="alphaRatio">開始時最終的な alpha 値の割合</param>
    /// <param name="isFaded">フェード終了フラグ</param>
    /// <param name="token">キャンセル処理用のトークン</param>
    /// <returns></returns>
    public async UniTask FadeOutImage(float fadeOutTime, float alphaRatio = 0.5f, bool isFaded = false, CancellationToken token = default)
    {
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
            await UniTask.Yield(token);
        }
    }

    /// <summary>
    /// フェードイン処理
    /// </summary>
    /// <param name="fadeInTime">フェードインにかかる秒数</param>
    /// <param name="alphaRatio">開始時の alpha 値の割合</param>
    /// <param name="isFaded">フェード終了フラグ</param>
    /// <param name="token">キャンセル処理用のトークン</param>
    /// <returns></returns>
    private async UniTask FadeInImage(float fadeInTime, float alphaRatio = 0.5f, bool isFaded = false, CancellationToken token = default)
    {
        Color imageColor = _backGroundImage.color;
        while (false == isFaded)
        {
            imageColor.a += Time.deltaTime * alphaRatio * (fadeInTime * 0.1f);
            _backGroundImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, imageColor.a);
            if (imageColor.a > 0.5f)
            {
                imageColor.a = 0.3f;
                isFaded = true;
            }
            await UniTask.Yield(token);
        }
    }
}
