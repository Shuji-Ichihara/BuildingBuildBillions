using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Conveyor : MonoBehaviour
{
    [SerializeField] private float DownSpeed = 10;
    public float previousTime;
    // �u���b�N�̗����鎞��
    public float fallTime = 1f;
    private bool _isHorizontalPressed = false;
    [SerializeField]
    float Rotateangle = 90;

    public enum PlayerNum
    {
        Player1 = 0,
        Player2 = 1
    }



    public float speed = 3.0f;
    private float diffX = 0.0f;
    readonly List<GameObject> gameObjects = new();



    void Start()
    {
        diffX = speed * Time.fixedDeltaTime;
    }






    private void FixedUpdate()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Vector2 speed = new Vector3(diffX, 0);
            Vector2 newPos = gameObject.GetComponent<Rigidbody2D>().position + speed;
            gameObject.GetComponent<Rigidbody2D>().position = newPos;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            gameObjects.Add(collision.gameObject);
        }
    }





    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObjects.Contains(collision.gameObject))
        {
            gameObjects.Remove(collision.gameObject);
        }
    }

    public PlayerNum jittai;

    private bool _isRightPressed = false;
    private bool _isLeftPressed = false;
    private bool _isHozirontalPressed = false;
    // �u���b�N��]
    public Vector3 rotationPoint;
    private bool Stop;

    private Vector2 _inputMove = Vector2.zero;
    //private bool rightwall;
    //private bool leftwall;
    private bool billstop;
    Rigidbody2D rb;
    bool pad = false;

    [SerializeField]
    float restTime = 0.25f;
    float restTime2 = 0.25f;
    float fromMoveHorizonal = 0.0f;

    //public Vector3 billControllerPosition { get; private set; }

    void Start1()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D1(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("stage"))
        {
            Stop = true;
            this.GetComponent<PlayerInput>().enabled = false;
        }
        if (collision.gameObject.CompareTag("Bill"))
        {

            billstop = true;
            this.GetComponent<PlayerInput>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("stage"))
        {
            Stop = true;
            this.GetComponent<PlayerInput>().enabled = false;
        }
        if (collision.gameObject.CompareTag("Bill"))
        {

            billstop = true;
            this.GetComponent<PlayerInput>().enabled = false;
        }
    }

    void Update()
    {
        //switch (jittai)
        //{
        //    case PlayerNum.Player1:
        //        restTime += Time.deltaTime;
        //        if (_isLeftPressed)
        //        {
        //            Debug.Log("ho");
        //            transform.Rotate(0, 0, 45);    //����������
        //        }
        //        if (_isRightPressed)
        //        {
        //            Debug.Log("ho2");
        //            transform.Rotate(0, 0, 45);    //�E��������
        //        }

        //        if (_inputMove.x != 0 && restTime >= 0.25f && _isHozirontalPressed == true)
        //        {
        //            transform.position += new Vector3(_inputMove.x * 50, 0, 0);
        //        }
        //        //��������
        //        transform.position += new Vector3(0, _inputMove.y * 50, 0);
        //        break;
        //    case PlayerNum.Player2:
        //        restTime += Time.deltaTime;
        //        if (_isLeftPressed)
        //        {
        //            Debug.Log("ho2");
        //            transform.Rotate(0, 0, 45);    //����������
        //        }
        //        if (_isRightPressed)
        //        {
        //            Debug.Log("ho222");
        //            transform.Rotate(0, 0, 45);    //�E��������
        //        }

        //        if (_inputMove.x != 0 && restTime >= 0.25f && _isHozirontalPressed == true)
        //        {
        //            transform.position += new Vector3(_inputMove.x * 50, 0, 0);
        //        }
        //        //��������
        //        transform.position += new Vector3(0, _inputMove.y * 50, 0);
        //        break;
        //}

        if (_isLeftPressed)
        {
            Debug.Log("RightRotate");
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -Rotateangle);
        }
        if (_isRightPressed)
        {
            Debug.Log("LeftRotate");
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), Rotateangle);
        }

        if (pad == true && _inputMove.x * _inputMove.x >= 0.25f)
        {
            float moveDistance = 50.0f;
            if (_inputMove.x < 0) moveDistance *= -1;

            var screenPoint = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(moveDistance, 0, 0));// 0,0~1.1

            if (CameraControllerTest.Instance.Camera.orthographicSize < 1080.0f * 1.5f)
                switch (jittai)
                {
                    case PlayerNum.Player1:
                        if (screenPoint.x >= 0f && screenPoint.x <= 0.45f)

                            if (_inputMove.x != 0)
                            {
                                Debug.Log("RightLeft");
                                _isHorizontalPressed = false;
                                transform.position += new Vector3(moveDistance, 0, 0);

                            }

                        pad = false;
                        break;
                    case PlayerNum.Player2:
                        //if (screenPoint.x >= 0.55 && screenPoint.x <= 0.8)
                        //    if (_isLeftPressed)
                        //    {
                        //        Debug.Log("RightRotate");
                        //        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -Rotateangle);
                        //    }
                        //if (_isRightPressed)
                        //{
                        //    Debug.Log("LeftRotate");
                        //    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), Rotateangle);
                        //}
                        if (screenPoint.x >= 0.55f && screenPoint.x <= 1f)

                            if (_inputMove.x != 0)
                            {
                                Debug.Log("RightLeft");
                                _isHorizontalPressed = false;
                                transform.position += new Vector3(moveDistance, 0, 0);

                            }

                        pad = false;
                        break;
                }
        }
        if (Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -50.0f, 0);
            previousTime = Time.time;
        }
        if (Stop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            //transform.position = new Vector3(transform.position.x, 1, 0);//���W��(0,1)�ɖ߂�
            this.enabled = false;
        }
        if (billstop == true)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);//���W�����̏�ɂƂǂ܂�

            this.enabled = false;
        }

        fromMoveHorizonal += Time.deltaTime;

        if (fromMoveHorizonal >= restTime)
        {
            pad = true;
            fromMoveHorizonal = 0.0f;
        }

        //Debug.Log(k);
        //if (pad == true && _inputMove.x * _inputMove.x >= 0.25f)
        //{
        //    float moveDistance = 50.0f;
        //    if (_inputMove.x < 0) moveDistance *= -1;

        //    var screenPoint2P = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(moveDistance, 0, 0));// 0,0~1.1

        //    if (CameraControllerTest.Instance.Camera.orthographicSize < 1080.0f * 1.5f)
        //    {
        //        Debug.Log("f");
        //        if (screenPoint2P.x >= 0.2f && screenPoint2P.x <= 0.45f)
        //        {
        //            if (_isLeftPressed)
        //            {
        //                Debug.Log("RightRotate");
        //                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -Rotateangle);
        //            }
        //            if (_isRightPressed)
        //            {
        //                Debug.Log("LeftRotate");
        //                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), Rotateangle);
        //            }
        //            if (_inputMove.x != 0 && fromMoveHorizonal >= restTime && _isHorizontalPressed == true)
        //            {
        //                Debug.Log("RightLeft");
        //                _isHorizontalPressed = false;
        //                transform.position += new Vector3(moveDistance, 0, 0);

        //            }
        //        }
        //    }
        //    transform.position += new Vector3(moveDistance, 0, 0);

        //    pad = false;
        //}

        transform.position += new Vector3(0, Mathf.Abs(_inputMove.y) * -DownSpeed, 0);


    }

    public void _Rotate(InputAction.CallbackContext context)
    {
        //transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        var y = context.control.name;
        Debug.Log(y);
        switch (context.phase)
        {
            case InputActionPhase.Performed:

                if (y == "leftShoulder")
                {
                    _isLeftPressed = true;
                }
                if (y == "rightShoulder")
                {
                    _isRightPressed = true;
                }
                break;
            case InputActionPhase.Canceled:
                _isLeftPressed = false;
                _isRightPressed = false;
                break;
        }
    }
    public void OnHold(InputAction.CallbackContext context)
    {
        _inputMove = context.ReadValue<Vector2>();
        Debug.Log(_inputMove);
        _isHorizontalPressed = true;
    }

    /// <summary>
    /// �u���b�N�ړ��֐�
    /// </summary>
    /// <param name="inputhorizotal"></param>
    /// <param name="inputvertical"></param>
    /// <param name="Player"> false =1p true=2p</param>
    //public void BillMovememt(InputAction.CallbackContext context)
    //{
    //    var k = context.ReadValue<Vector2>();
    //    //Debug.Log(k);
    //    if (pad == true && k.x * k.x >= 0.15f)
    //    {
    //        float moveDistance = 50.0f;
    //        if (k.x < 0) moveDistance *= -1;

    //        var screenPoint2P = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(moveDistance, 0, 0));// 0,0~1.1

    //        if (screenPoint2P.x <= 0.48 && screenPoint2P.x >= 0)

    //            transform.position += new Vector3(moveDistance, 0, 0);

    //            pad = false;
    //    }
    //    // var screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);// 0,0~1.1

    //    // �����ŉ��Ɉړ������A�����L�[�ł��ړ�����
    //    if (pad == true && k.y * k.y >= 0.25f || Input.GetKeyDown(KeyCode.S))
    //    {
    //        if (k.y <= 0.25f)
    //        {
    //            transform.position += new Vector3(0, Mathf.Sign(k.y) * 50.0f, 0);
    //            pad = false;
    //        }
    //    }

    //}
    void OnDisable()
    {
        switch (jittai)
        {
            case PlayerNum.Player1:
                this.GetComponent<PlayerInput>().enabled = false;
                FindObjectOfType<SpownBill>().NewBill();//�V�����r�������X�|�[��
                                                        // ���̃I�u�W�F�N�g������������鎞�ɁA���g�̍��W���i�[
                GameManager.Instance.SpownBill.BuildingPosition = transform.position;
                break;
            case PlayerNum.Player2:
                this.GetComponent<PlayerInput>().enabled = false;
                FindObjectOfType<SpownBill2P>().NewBill2P();//�V�����r�������X�|�[��
                                                            // ���̃I�u�W�F�N�g������������鎞�ɁA���g�̍��W���i�[
                GameManager.Instance.SpownBill.BuildingPosition = transform.position;
                break;

                this.GetComponent<PlayerInput>().enabled = false;
                FindObjectOfType<SpownBill>().NewBill();//�V�����r�������X�|�[��
                                                        // ���̃I�u�W�F�N�g������������鎞�ɁA���g�̍��W���i�[
                GameManager.Instance.SpownBill.BuildingPosition = transform.position;
        }
    }

    public void FreezeAllConstraints(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}


