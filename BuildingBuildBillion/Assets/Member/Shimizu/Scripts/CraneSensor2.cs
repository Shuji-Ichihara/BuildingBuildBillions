using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneSensor2 : MonoBehaviour
{
    [Header("�A�[���ړ���(�c)"),SerializeField]
    private float width = 1;
    [SerializeField]
    DistanceJoint2D distanceJoint2D;
    [SerializeField]
    CraneJib craneJib;

    private Vector2 defaultPosition; //�f�t�H���g�|�W�V�����i�[�p

    private bool armCanMove = false; //�A�[�����ړ����邩�ǂ���
    private bool armCatch = false;
    //private bool isCatch = false;

    private float pickUpTime = 3;

    private GameObject parentObj;
    private GameObject otherObject;

    private float otherObjStartPos;

    BoxCollider2D boxCollider2D;
    //���p
    public bool widthExtend = false;
    public bool widthContract = false;
    private float speedWidth = 0;
    private Vector2 defaultPositionParent;
    private float endPositionWidth = 0;
    //�c
    private float startPosition;
    private float endPosition;
    private float speed = 5f;
    private float ratio = 0;
    private float ratio2 = 0;
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.localPosition;
        defaultPositionParent = transform.parent.localPosition;
        parentObj = transform.parent.gameObject;
        boxCollider2D = GetComponent<BoxCollider2D>();
        endPosition = defaultPosition.y - width;
        //
        speedWidth = craneJib.ExtendSpeed();
        endPositionWidth = craneJib.ExtendDistance();
    }

    // Update is called once per frame
    void Update()
    {
        //��
        //�L�΂�
        if(widthExtend)
        {
            transform.parent.localPosition = new Vector2(Mathf.Lerp(defaultPositionParent.x, -endPositionWidth + defaultPositionParent.x, ratio),transform.parent.localPosition.y);
            ratio += Time.deltaTime / speedWidth;
            if (ratio > 1f)
            {
                widthExtend = false;
                ratio = 0;
            }
        }
        //�k�߂�
        if (widthContract)
        {
            //Debug.Log(defaultPositionParent.x);
            transform.parent.localPosition = new Vector2(Mathf.Lerp(-endPositionWidth + defaultPositionParent.x, defaultPositionParent.x - 8.5f, ratio2), transform.parent.localPosition.y);
            ratio2 += Time.deltaTime / speedWidth;
            if (ratio2 > 1f)
            {
                widthContract = false;
                ratio2 = 0;
            }
        }
        //�c
        if (armCanMove)
        {
            //Debug.Log(defaultPosition.y);
            transform.localPosition = new Vector2(transform.localPosition.x, Mathf.Lerp(defaultPosition.y, endPosition, ratio));
            ratio += Time.deltaTime / speed;
            if (ratio > 1)
            {
                armCanMove = false;
                ratio = 0;
            }
        }
        else if (armCatch)
        {
            transform.localPosition = otherObject.transform.localPosition;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bill")||  (collision.gameObject.CompareTag("Bill2")))
        { 
            armCanMove = false;
            armCatch = true;
            otherObject = collision.gameObject;
            //boxCollider2D.enabled = false;

            //�N���[���I�u�W�F�N�g�̎q�ɂ���
            collision.gameObject.transform.parent = parentObj.transform;
            //Destroy(collision.gameObject.GetComponent<Rigidbody2D>());
            collision.gameObject.GetComponent<BoxCollider2D>().usedByComposite = true;
            //�W���C���g�ݒ�
            distanceJoint2D.enabled = true;
            distanceJoint2D.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            distanceJoint2D.connectedAnchor = collision.gameObject.transform.InverseTransformPoint(this.transform.position);
            StartCoroutine(ConfigureDistance());
            //Debug.Log("���񂾂�");

        }
    }
    public void CraneMoveOn()
    {
        armCanMove = true;
    }
    IEnumerator ConfigureDistance()
    {
        var sumTime = 0f;
        while (true)
        {
            sumTime += Time.deltaTime;
            var ratio = sumTime / pickUpTime;

            distanceJoint2D.distance = Mathf.Lerp(15f, 0.5f, ratio);
            if (ratio > 1f)
            {
                craneJib.CanContract();
                break;
            }
            yield return null;
        }
    }
}
