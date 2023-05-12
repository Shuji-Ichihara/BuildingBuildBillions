using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerTest : MonoBehaviour
{
    // メインカメラ
    [SerializeField]
    private Camera _mainCamera = null;

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
        Debug.Log(_mainCamera.orthographicSize);
        MainCameraMove();

        // mainCam.orthographicSizeは0だとエラー出るっぽいので回避策
        if (_mainCamera.orthographicSize >= 540 && _mainCamera.orthographicSize <= 540 * 2)
        {
            _mainCamera.orthographicSize += scroll;
            _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize
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
            _mainCamera.transform.position += Vector3.up * cameraMoveYValue * Time.deltaTime * 100;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _mainCamera.transform.position += Vector3.down * cameraMoveYValue * Time.deltaTime * 100;
        }
        // カメラの Y 座標の移動範囲を制限
        _mainCamera.transform.position = new Vector3(0.0f
                                                   , Mathf.Clamp(_mainCamera.transform.position.y
                                                               , 0.0f
                                                               , Screen.height / 2.0f)
                                                   , _mainCamera.transform.position.z);
    }
}
