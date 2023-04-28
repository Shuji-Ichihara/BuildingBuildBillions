using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerTest : MonoBehaviour
{
    // ���C���J����
    private Camera _mainCamera = null;

    private float _mouseZoomSpeed = 15.0f;
    private float _zoomMinBound = 0.1f;
    private float _zoomMaxBound = 179.9f;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //�z�C�[�����擾���āA�ς��̂��߂�time.deltaTime�������Ă���
        var scroll = Input.mouseScrollDelta.y * Time.deltaTime * 100;
        Debug.Log(_mainCamera.orthographicSize);

        //mainCam.orthographicSize��0���ƃG���[�o����ۂ��̂ŉ����
        if (_mainCamera.orthographicSize >= 540 && _mainCamera.orthographicSize <= 540 * 2)
        {
            _mainCamera.orthographicSize += scroll;
            _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize
                                                     , 540
                                                     , 540 * 2);
        }
    }

    private void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Zoom(scroll, _mouseZoomSpeed);

    }

    private void Zoom(float deltaMagnitudeDiff, float speed)
    {
        _mainCamera.orthographicSize += deltaMagnitudeDiff * speed;
        _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.fieldOfView
                                            , _zoomMinBound
                                            , _zoomMaxBound);
    }
}
