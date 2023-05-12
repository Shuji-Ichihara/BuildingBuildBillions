using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerTest : MonoBehaviour
{
    // メインカメラ
    [SerializeField]
    private Camera _camera = null;

    // Start is called before the first frame update
    void Start()
    {
        //_mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // カメラズームのテストプログラム
        var scroll = Input.GetAxis("Vertical") * Time.deltaTime * 100;
        Debug.Log(_camera.orthographicSize);
        MainCameraMove();

        // mainCam.orthographicSizeは0だとエラー出るっぽいので回避策
        if (_camera.orthographicSize >= 540 && _camera.orthographicSize <= 540 * 2)
        {
            _camera.orthographicSize += scroll;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize
                                                     , 540
                                                     , 540 * 2);
        }

    }

    private void MainCameraMove()
    {
        float cameraMoveYValue = 1.0f;
        // カメラの z 座標を移動
        if (Input.GetKey(KeyCode.A))
        {
            _camera.transform.position += Vector3.up * cameraMoveYValue * Time.deltaTime * 100;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _camera.transform.position += Vector3.down * cameraMoveYValue * Time.deltaTime * 100;
        }
        // カメラの Y 座標の移動範囲を制限
        _camera.transform.position = new Vector3(0.0f
                                                   , Mathf.Clamp(_camera.transform.position.y
                                                               , 0.0f
                                                               , Screen.height / 2.0f)
                                                   , _camera.transform.position.z);
    }
}
