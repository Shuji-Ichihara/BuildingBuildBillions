using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneSensor2 : MonoBehaviour
{
    [Header("アーム移動幅")]
    [SerializeField]
    private float width = 1;
    [SerializeField]
    DistanceJoint2D distanceJoint2D;


    private Vector2 defaultPosition; //デフォルトポジション格納用

    private bool armCanMove = false; //アームが移動するかどうか
    private bool armCatch = false;

    private float pickUpTime = 3;

    private GameObject parentObj;
    private GameObject otherObject;

    BoxCollider2D boxCollider2D;

    private float startPosition;
    private float endPosition;
    private float speed = 5f;
    private float ratio = 0;
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.localPosition;
        parentObj = transform.parent.gameObject;
        boxCollider2D = GetComponent<BoxCollider2D>();
        endPosition = defaultPosition.y - width;
    }

    // Update is called once per frame
    void Update()
    {

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
        else if(armCatch)
        {
            transform.position = otherObject.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Materials"))
        {
            armCanMove = false;
            armCatch = true;
            otherObject = collision.gameObject;
            boxCollider2D.enabled = false;
            //クレーンオブジェクトの子にする
            collision.gameObject.transform.parent = parentObj.transform;

            distanceJoint2D.enabled = true;
            distanceJoint2D.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            StartCoroutine(ConfigureDistance());
            //Debug.Log("つかんだよ");


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

            distanceJoint2D.distance = Mathf.Lerp(6f, 0.5f, ratio);
            if (ratio > 1f)
            {
                break;
            }
            yield return null;
        }
    }
}
