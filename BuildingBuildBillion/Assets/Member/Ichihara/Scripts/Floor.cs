using UnityEngine;

public class Floor : MonoBehaviour
{
    private bool _isContactObject = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (false == _isContactObject && other.gameObject.CompareTag("Bill") || other.gameObject.CompareTag("Bill2"))
        {
            CameraControllerTest.Instance.CallCalucrateCameraMovement();
            _isContactObject = true;
        }
    }
}
