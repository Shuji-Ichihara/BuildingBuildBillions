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
        //ホイールを取得して、均しのためにtime.deltaTimeをかけておく
        var scroll = Input.GetAxis("Vertical") * Time.deltaTime * 100;
        Debug.Log(_mainCamera.orthographicSize);
        MainCameraMove(scroll);

        // mainCam.orthographicSizeは0だとエラー出るっぽいので回避策
        if (_mainCamera.orthographicSize >= 540 && _mainCamera.orthographicSize <= 540 * 2)
        {
            _mainCamera.orthographicSize += scroll;
            _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize
                                                     , 540
                                                     , 540 * 2);
        }

    }

    private void MainCameraMove(float mouseScrollWheel)
    {
        // カメラの z 座標を移動
        _mainCamera.transform.position += new Vector3(0, 0, mouseScrollWheel);
        Debug.Log($"{_mainCamera.transform.position.z}");
    }
}
