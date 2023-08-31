using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    [Header("カメラのズーム")]
    // ズームさせるカメラ
    [SerializeField]
    private CinemachineVirtualCamera _camera = null;
    public CinemachineVirtualCamera Camera => _camera;
    // カメラズームのスピードの変化量
    [SerializeField]
    private float _zoomCameraSpeed = 80.0f;
    // カメラの Y 軸方向の変化量
    [SerializeField]
    private float _moveCameraSpeed = 100.0f;
    // Cinemachine の TargetGroup
    [SerializeField]
    private GameObject _empty = null;
    private Vector3 _defaultPosition = Vector3.zero;

    private CancellationTokenSource _cts = new CancellationTokenSource();
    private Func<GameObject, GameObject, float> ObjectTop;

    // Start is called before the first frame update
    void Start()
    {
        _camera.m_Lens.OrthographicSize = 540.0f;
        _defaultPosition = _empty.transform.position;
        ObjectTop += GetObjectTop;
    }

    /// <summary>
    /// CalucrateCameraMovement を呼び出す
    /// </summary>
    public void CallCalucrateCameraMovement()
    {
        CalucrateCameraMovement(_cts).Forget();
    }

    /// <summary>
    /// カメラズーム
    /// </summary>
    /// <param name="cts">キャンセル処理用のトークン</param>
    private async UniTask CalucrateCameraMovement(CancellationTokenSource cts = default)
    {
        // カメラの OrthographicSize の変化量
        float zoom = _zoomCameraSpeed * Time.deltaTime;
        // カメラの Y 座標の移動方向を格納する変数
        // 初期値が最低値の為、Vector3.up で初期化
        Vector3 moveVector = Vector3.up;
        // 判定バーの移動方向の設定
        // 初期値が最低値の為、true で初期化
        bool isMovedCameraSwtich = true;
        while (_camera.m_Lens.OrthographicSize >= 540 && _camera.m_Lens.OrthographicSize <= Screen.height)
        {
            // 要変更
            if (GameManager.Instance.CountDownGameTime < 0.0f) { break; }
            // カメラがズームアウトするのに必要なビルの高さ
            float needBuildingTop = _camera.m_Lens.OrthographicSize * GameManager.Instance.BuildingHeightAndScreenRatio;
            float buildingTop = GetBuildingTop().y;
            // ビルの高さが buildingTop 以上であればズームアウト
            if (buildingTop > needBuildingTop)
            {
                if (zoom < 0.0f) { zoom *= -1; }
                moveVector = Vector3.up;
                isMovedCameraSwtich = true;
            }
            // ビルの高さが buildingTop より低ければズームイン
            else if (buildingTop < needBuildingTop)
            {
                if (zoom > 0.0f) { zoom *= -1; }
                moveVector = Vector3.down;
                isMovedCameraSwtich = false;
            }
            else { continue; }
            await MoveCamera(zoom, moveVector, isMovedCameraSwtich, cts);
            // カメラの OrthgraphicSize の限界値を定義
            _camera.m_Lens.OrthographicSize = Mathf.Clamp(_camera.m_Lens.OrthographicSize
                                                     , Screen.height / 2.0f
                                                     , Screen.height); ;
            await UniTask.Yield(cts.Token);
        }

    }

    /// <summary>
    /// 現在操作しているオブジェクトの座標を比較
    /// </summary>
    /// <param name="obj">Player1 が操作しているオブジェクト</param>
    /// <param name="obj2">Player2 が操作しているオブジェクト</param>
    /// <returns></returns>
    private float GetObjectTop(GameObject obj, GameObject obj2)
    {
        if (GameManager.Instance.CountDownGameTime < 0.0f)
        {
            ObjectTop -= GetObjectTop;
            return 0;
        }
        float top = 0.0f;
        if (obj.transform.position.y > obj2.transform.position.y)
        {
            top = obj.transform.position.y;
        }
        else if (obj.transform.position.y < obj2.transform.position.y)
        {
            top = obj2.transform.position.y;
        }
        return top;
    }

    /// <summary>
    /// カメラの Y 軸移動
    /// </summary>
    /// <param name="zoom">カメラズームの変化量</param>
    /// <param name="vector">カメラの Y 軸の移動方向</param>
    /// <param name="movedSwitch">カメラの移動方向の切り替え</param>
    /// <param name="cts">UniTask 中止用のトークン</param>
    /// <returns></returns>
    private async UniTask MoveCamera(float zoom, Vector3 vector, bool movedSwitch, CancellationTokenSource cts = default)
    {
        // 変更の可能性有
        await UniTask.WaitForFixedUpdate(cts.Token);
        try
        {
            // カメラのズーム、移動を止める処理
            float top = ObjectTop(GameManager.Instance.Obj, GameManager.Instance.Obj2);
            if (ObjectTop == null)
            {
                cts.Cancel();
                return;
            }
            if (_empty.transform.position.y - top
                > _camera.m_Lens.OrthographicSize * GameManager.Instance.BuildingHeightAndScreenRatio / 2.0f) { return; }
            _empty.transform.position += new Vector3(0.0f, zoom, 0.0f);
            _empty.transform.position = new Vector3(_empty.transform.position.x
                                                  , Mathf.Clamp(_empty.transform.position.y
                                                              , _defaultPosition.y
                                                              , Screen.height * 1.5f - (Screen.height / 2.0f - _defaultPosition.y))
                                                  , 0.0f);
            // カメラの Y 座標の移動
            _camera.transform.position += vector * _moveCameraSpeed;
            JadgementBarController.Instance.MoveJadgementBarFallPoint(movedSwitch);
            GameManager.Instance.MoveBuildingSpawnPoint(zoom);
            // カメラの Y 座標の移動限界を定義
            _camera.transform.position = new Vector3(0.0f
                                                   , Mathf.Clamp(_camera.transform.position.y
                                                                , 0.0f
                                                                , Screen.height / 2.0f)
                                                   , _camera.transform.position.z);
        }
        catch (MissingReferenceException mre)
        {
            ObjectTop -= GetObjectTop;
            throw;
        }
    }

    /// <summary>
    /// 積みあがっている建材オブジェクトで、一番 Y 座標が大きい建材オブジェクトを検索する
    /// </summary>
    /// <returns></returns>
    private Vector3 GetBuildingTop()
    {
        Vector3 buildingTop = Vector3.zero;
        // 接地したオブジェクトを検索
        Vector3 dummy1Position = GameManager.Instance.SpownBill.BuildingPosition;
        Vector3 dummy2Position = GameManager.Instance.SpownBill2P.BuildingPosition;
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
