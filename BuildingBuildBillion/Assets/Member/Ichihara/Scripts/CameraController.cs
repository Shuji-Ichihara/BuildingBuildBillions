using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    [Header("カメラのズーム")]
    // ズームさせるカメラ
    [SerializeField]
    private Camera _camera = null;
    public Camera Camera => _camera;
    // カメラズームのスピードの変化量
    [SerializeField]
    private float _zoomCameraSpeed = 80.0f;
    // カメラの Y 軸方向の変化量
    [SerializeField]
    private float _moveCameraSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        _camera.orthographicSize = 540.0f;
    }

    /// <summary>
    /// CalucrateCameraMovement を呼び出す
    /// </summary>
    public void CallCalucrateCameraMovement()
    {
        CalucrateCameraMovement(this.GetCancellationTokenOnDestroy()).Forget();
    }

    /// <summary>
    /// カメラズーム
    /// </summary>
    private async UniTask CalucrateCameraMovement(CancellationToken token = default)
    {
        // カメラの OrthographicSize の変化量
        float zoom = _zoomCameraSpeed * Time.deltaTime;
        // カメラの Y 座標の移動方向を格納する変数
        // 初期値が最低値の為、Vector3.up で初期化
        Vector3 moveVector = Vector3.up;
        // 判定バーの移動方向の設定
        // 初期値が最低値の為、true で初期化
        bool isMovedCameraSwtich = true;
        while (_camera.orthographicSize >= 540 && _camera.orthographicSize <= Screen.height)
        {
            // 要変更
            if (GameManager.Instance.CountDownGameTime < 0.0f) { break; }
            // カメラがズームアウトするのに必要なビルの高さ
            float buildingTop = _camera.orthographicSize * GameManager.Instance.BuildingHeightAndScreenRatio;
            // ビルの高さが buildingTop 以上であればズームアウト
            //Debug.Log($"ズームアウト : {GetBuildingTop().y > buildingTop}, {GetBuildingTop()}, {zoom}");
            //Debug.Log($"ズームイン : {GetBuildingTop().y < buildingTop}, {GetBuildingTop()}, {zoom}");
            if (GetBuildingTop().y > buildingTop)
            {
                if (zoom < 0.0f) { zoom *= -1; }
                moveVector = Vector3.up;
                isMovedCameraSwtich = true;
            }
            // ビルの高さが buildingTop より低ければズームイン
            else if (GetBuildingTop().y < buildingTop)
            {
                if (zoom > 0.0f) { zoom *= -1; }
                moveVector = Vector3.down;
                isMovedCameraSwtich = false;
            }
            else { continue; }
            await MoveCamera(zoom, moveVector, isMovedCameraSwtich, token);
            // カメラの OrthgraphicSize の限界値を定義
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize
                                                     , 540.0f
                                                     , Screen.height);
        }
    }

    /// <summary>
    /// カメラの Y 軸移動
    /// </summary>
    /// <param name="zoom">カメラズームの変化量</param>
    /// <param name="vector">カメラの Y 軸の移動方向</param>
    /// <param name="movedSwitch">カメラの移動方向の切り替え</param>
    /// <param name="token">UniTask 中止用のトークン</param>
    /// <returns></returns>
    private async UniTask MoveCamera(float zoom, Vector3 vector, bool movedSwitch, CancellationToken token = default)
    {
        // 変更の可能性有
        await UniTask.WaitForFixedUpdate(token);
        _camera.orthographicSize += zoom;
        _camera.transform.position += vector * _moveCameraSpeed * Time.deltaTime;
        JadgementBarController.Instance.MoveJadgementBarFallPoint(movedSwitch);
        GameManager.Instance.MoveBuildingSpawnPoint();
        // カメラの Y 座標の移動限界を定義
        _camera.transform.position = new Vector3(0.0f
                                               , Mathf.Clamp(_camera.transform.position.y
                                                            , 0.0f
                                                            , Screen.height / 2.0f)
                                               , _camera.transform.position.z);
    }

    /// <summary>
    /// 積みあがっている建材オブジェクトで、一番 Y 座標が大きい建材オブジェクトを検索する
    /// </summary>
    /// <returns></returns>
    private Vector3 GetBuildingTop(Vector3 buildingTop = default)
    {
        // 接地したオブジェクトを検索
        var dummy1Position = GameManager.Instance.SpownBill.BuildingPosition;
        var dummy2Position = GameManager.Instance.SpownBill2P.BuildingPosition;
        // 勝敗判定時、 MissingReferenceException が発生する場合がある為、例外処理を行う
        try
        {
            if (dummy1Position.y > dummy2Position.y)
            {
                buildingTop = dummy1Position;
            }
            else if (dummy1Position.y < dummy2Position.y)
            {
                buildingTop = dummy2Position;
            }

        }
        catch (MissingReferenceException mre)
        {
            Debug.Log(mre + "無視していいよ");
            throw;
        }
        return buildingTop;
    }
}
