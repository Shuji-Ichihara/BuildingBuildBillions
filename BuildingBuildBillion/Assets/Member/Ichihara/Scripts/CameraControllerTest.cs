using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class CameraControllerTest : SingletonMonoBehaviour<CameraControllerTest>
{
    [Header("カメラのズーム")]
    // ズームさせるカメラ
    [SerializeField]
    private Camera _camera = null;
    // カメラズームのスピードの変化量
    [SerializeField]
    private float _zoomCameraSpeed = 65.0f;
    // カメラの Y 軸方向の変化量
    [SerializeField]
    private float _moveCameraSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        _camera.orthographicSize = 540.0f;
    }

    public void CallCalucrateCameraMovement()
    {
        CalucrateCameraMovement(this.GetCancellationTokenOnDestroy()).Forget();
    }

    /// <summary>
    /// カメラズームのプログラム
    /// </summary>
    private async UniTask CalucrateCameraMovement(CancellationToken token = default)
    {
        // カメラの OrthographicSize の変化量
        float zoom = _zoomCameraSpeed * Time.deltaTime;

        // カメラの Y 座標の移動方向を格納する変数
        // 初期値が最低値の為、Vector3.up を代入
        Vector3 moveVector = Vector3.up;
        // 判定バーの移動方向の設定
        // 初期値が最低値の為、true を代入
        bool isMoveCameraSwtich = true;
        while(_camera.orthographicSize >= 540 && _camera.orthographicSize <= Screen.height)
        {
            // I キーを押したらズームイン
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (zoom < 0.0f) { zoom *= -1; }
                moveVector = Vector3.up;
                isMoveCameraSwtich = true;
            }
            // O キーを押したらズームアウト
            else if (Input.GetKeyDown(KeyCode.O))
            {
                zoom *= -1;
                moveVector = Vector3.down;
                isMoveCameraSwtich = false;
            }
            await MoveCamera(zoom, moveVector, isMoveCameraSwtich, token);
            // カメラの Orthgraphic の限界値を定義
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize
                                                     , 540.0f
                                                     , Screen.height);
        }
    }

    private async UniTask MoveCamera(float zoom, Vector3 vector, bool moveSwitch, CancellationToken token = default)
    {
        _camera.orthographicSize += zoom;
        _camera.transform.position += vector * _moveCameraSpeed * Time.deltaTime;
        JadgementBarControllerTest.Instance.MoveJadgementBarFallPoint(moveSwitch);
        // カメラの Y 座標の移動限界を定義
        _camera.transform.position = new Vector3(0.0f
                                               , Mathf.Clamp(_camera.transform.position.y
                                                            , 0.0f
                                                            , Screen.height / 2.0f)
                                               , _camera.transform.position.z);
        // 変更の可能性有
        await UniTask.Yield(token);
    }
    // 自動でカメラズームを制御
}
