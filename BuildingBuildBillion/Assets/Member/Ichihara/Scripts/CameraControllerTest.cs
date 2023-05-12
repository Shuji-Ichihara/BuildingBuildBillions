using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraControllerTest : MonoBehaviour
{
    // ズームさせるカメラ
    [SerializeField]
    private Camera _camera = null;
    // カメラズームのスピード
    [SerializeField]
    private float _zoomSpeed = 100.0f;

    // Start is called before the first frame update
    async void Start()
    {
        _camera.orthographicSize = 540.0f;
        await CalucrateCameraMovement(new CancellationToken());
    }

    // Update is called once per frame
    void Update()
    {
        //await ZoomCamera(_token);
    }

    /// <summary>
    /// カメラズームのプログラム
    /// </summary>
    private async UniTask CalucrateCameraMovement(CancellationToken token)
    {
        // カメラの OrthographicSize の変化量
        float zoom = _zoomSpeed * Time.deltaTime;
        // カメラの Y 座標の変化量
        float cameraMoveYValue = 100.0f;

        /*
        // カメラの Orthgraphic 野初期値は 540 を想定
        if (_camera.orthographicSize >= 540 && _camera.orthographicSize <= Screen.height)
        {
            // 押したボタンに応じてズームイン、ズームアウト
            if (Input.GetKeyDown(KeyCode.I))
            {
                // I キーを押したらズームイン
                if (zoom < 0.0f) { zoom *= -1; }

                while (_camera.orthographicSize > 540 || _camera.orthographicSize < Screen.height)
                {
                    _camera.orthographicSize += zoom;
                    _camera.transform.position += Vector3.up * cameraMoveYValue * Time.deltaTime;
                    GameManager.Instance.MoveJadgementBarFallPoint(true);
                    await UniTask.Yield();
                }
            }
            // O キーを押したらズームアウト
            else if (Input.GetKeyDown(KeyCode.O))
            {
                zoom *= -1;
                while (_camera.orthographicSize > 540 || _camera.orthographicSize < Screen.height)
                {
                    _camera.orthographicSize += zoom;
                    _camera.transform.position += Vector3.down * cameraMoveYValue * Time.deltaTime;
                    GameManager.Instance.MoveJadgementBarFallPoint(false);
                    await UniTask.Yield();
                }
            }
            // カメラの Orthgraphic の限界値を定義
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize
                                                     , 540
                                                     , Screen.height);
            // カメラの Y 座標の移動範囲を定義
            _camera.transform.position = new Vector3(0.0f
                                                   , Mathf.Clamp(_camera.transform.position.y
                                                                , 0.0f
                                                                , Screen.height / 2.0f)
                                                   , _camera.transform.position.z);
        }*/

        // カメラの Y 座標の移動方向を格納する変数
        Vector3 dummyVector3 = Vector3.zero;
        // 判定バーの移動方向の設定
        bool moveCameraSwtich = false;
        while(_camera.orthographicSize >= 540 && _camera.orthographicSize <= Screen.height)
        {
            // I キーを押したらズームイン
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (zoom < 0.0f) { zoom *= -1; }
                dummyVector3 = Vector3.up;
                moveCameraSwtich = true;
            }
            // O キーを押したらズームアウト
            else if (Input.GetKeyDown(KeyCode.O))
            {
                zoom *= -1;
                dummyVector3 = Vector3.down;
                moveCameraSwtich = false;
            }
            await MoveCamera(zoom, dummyVector3, cameraMoveYValue, moveCameraSwtich, new CancellationToken());
            // カメラの Orthgraphic の限界値を定義
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize
                                                     , 540.0f
                                                     , Screen.height);
        }
    }

    private async UniTask MoveCamera(float zoom, Vector3 vector, float MoveY, bool moveSwitch, CancellationToken token)
    {
        _camera.orthographicSize += zoom;
        _camera.transform.position += vector * MoveY;
        GameManager.Instance.MoveJadgementBarFallPoint(moveSwitch);
        // カメラの Y 座標の移動範囲を定義
        _camera.transform.position = new Vector3(0.0f
                                               , Mathf.Clamp(_camera.transform.position.y
                                                            , 0.0f
                                                            , Screen.height / 2.0f)
                                               , _camera.transform.position.z);
        await UniTask.Yield();
    }
    // 自動でカメラズームを制御
}
