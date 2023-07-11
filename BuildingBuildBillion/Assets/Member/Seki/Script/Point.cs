using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Point : MonoBehaviour
{

    private DistanceJoint2D dis,parentDis = null;
    // Start is called before the first frame update
    void Start()
    {
        dis = GetComponent<DistanceJoint2D>();
        parentDis = transform.parent.gameObject.GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.contacts[0].point);
        /*
        if(collision.gameObject.CompareTag("Player"))
        {
            dis.enabled = true;
            dis.connectedBody =
                    collision.gameObject.GetComponent<Rigidbody2D>();
            foreach (ContactPoint2D point in collision.contacts)
            {
                
                dis.anchor = 
                    this.gameObject.transform.InverseTransformPoint(point.point);
                dis.connectedAnchor =
                    collision.gameObject.transform.InverseTransformPoint(point.point);
            }
        }*/
       
    }

    private void OnEnable()
    {
        this.transform.localPosition = new Vector3(0,0,0);
        parentDis = transform.parent.gameObject.GetComponent<DistanceJoint2D>();
        parentDis.connectedBody = this.gameObject.GetComponent<Rigidbody2D>();
    }
    private void OnDisable()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        parentDis.connectedBody = null;
        dis.connectedBody = null;
        dis.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dis.enabled = true;
            dis.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            dis.connectedAnchor =
                    collision.gameObject.transform.InverseTransformPoint(this.transform.position);

        }
    }
}
