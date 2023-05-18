using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneSensor : MonoBehaviour
{
    [Header("�A�[���ړ���")]
    [SerializeField]
    private float width = 1;
    [Header("�A�[���ړ�����")]
    [SerializeField]
    private float period = 2;
    [SerializeField]
    DistanceJoint2D distanceJoint2D;

    private float pickUpTime = 3;
    

    private Vector3 defaultPosition; //�f�t�H���g�|�W�V�����i�[�p

    private bool craneMoving = false; //�A�[�����ړ����邩�ǂ���

    private GameObject parentObj;

    BoxCollider2D boxCollider2D;

    private void Awake()
    {
        defaultPosition = transform.localPosition;
        parentObj = transform.parent.gameObject;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (craneMoving)
        {
            var posY = Mathf.PingPong(4 * width * Time.time / period, 2 * width);
            transform.localPosition = defaultPosition - new Vector3(0, posY, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Materials"))
        {
            craneMoving = false;
            boxCollider2D.enabled = false;
            //���̃I�u�W�F�N�g�̎q�ɂ���
            collision.gameObject.transform.parent = parentObj.transform;

            distanceJoint2D.enabled = true;
            distanceJoint2D.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            StartCoroutine(ConfigureDistance());
            //Debug.Log("���񂾂�");

            
        }
    }
    IEnumerator ConfigureDistance()
    {
        var sumTime = 0f;
        while (true)
        {
            sumTime += Time.deltaTime;
            var ratio = sumTime / pickUpTime;

            distanceJoint2D.distance = Mathf.Lerp(6f, 0.5f, ratio);
            if(ratio > 1f)
            {
                break;
            }
            yield return null;
        }
    }
}
