using UnityEngine;

public class Floor : MonoBehaviour
{
    private bool _isContactedObject = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (false == _isContactedObject && other.gameObject.CompareTag("Bill") || other.gameObject.CompareTag("Bill2"))
        {
            CameraController.Instance.CallCalucrateCameraMovement();
            _isContactedObject = true;
        }
    }
}
