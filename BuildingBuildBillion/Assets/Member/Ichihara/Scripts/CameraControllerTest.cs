using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerTest : MonoBehaviour
{
    // メインカメラ
    [SerializeField]
    private Camera _camera = null;
    public Camera Camera => _camera;

    // Start is called before the first frame update
    void Start()
    {
        //_mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ZoomCamera();
    }

    /// <summary>
    /// カメラズームのプログラム
    /// </summary>
    private void ZoomCamera()
    {
        // カメラの OrthographicSize の変化量
        float zoom = Input.GetAxis("Vertical") * Time.deltaTime * 100;

        // mainCam.orthographicSizeは0だとエラー出るっぽいので回避策
        if (_camera.orthographicSize >= 540 && _camera.orthographicSize <= 540 * 2)
        {
            _camera.orthographicSize += zoom;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize
                                                     , 540
                                                     , Screen.height);
        }

        // カメラの Y 座標の移動量
        float cameraMoveYValue = 100.0f;
        // カメラの z 座標を移動
        if (zoom > 0.0f)
        {
            _camera.transform.position += Vector3.up * cameraMoveYValue * Time.deltaTime;
        }
        else if (zoom < 0.0f)
        {
            _camera.transform.position += Vector3.down * cameraMoveYValue * Time.deltaTime;
        }
        // カメラの Y 座標の移動範囲を制限
        _camera.transform.position = new Vector3(0.0f
                                               , Mathf.Clamp(_camera.transform.position.y
                                                            , 0.0f
                                                            , Screen.height / 2.0f)
                                               , _camera.transform.position.z);
        GameManager.Instance.MoveJadgementBarFallPoint(zoom);
    }
    // 自動でカメラズームを制御
    // ズームに応じてカメラの Y 座標を移動
}
