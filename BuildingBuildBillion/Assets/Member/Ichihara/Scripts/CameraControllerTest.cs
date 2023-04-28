using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerTest : MonoBehaviour
{
    // ÉÅÉCÉìÉJÉÅÉâ
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
        CameraZoom();
        Debug.Log($"{_mainCamera.fieldOfView}");
    }

    private void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Zoom(scroll, _mouseZoomSpeed);
        
    }

    private void Zoom(float deltaMagnitudeDiff, float speed)
    {
        _mainCamera.fieldOfView += deltaMagnitudeDiff * speed;
        _mainCamera.fieldOfView = Mathf.Clamp(_mainCamera.fieldOfView
                                            , _zoomMinBound
                                            , _zoomMaxBound);
    }
}
