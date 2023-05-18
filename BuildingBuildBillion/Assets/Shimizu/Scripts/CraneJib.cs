using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneJib : MonoBehaviour
{
    private float startPosition;
    private float endPosition = 4;
    private float speed = 2f;
    private float ratio = 0;
    //[System.NonSerialized]
    private bool jibExtend = false;
    [System.NonSerialized]
    public bool jibContract = false;
    [SerializeField]
    CraneSensor2 craneSensor;
    private void Start()
    {
        startPosition = transform.position.x;
        endPosition = transform.position.x + 3;
    }
    private void Update()
    {
        //Debug.Log(jibExtend);
        if(jibExtend)
        {
            transform.position = new Vector2(Mathf.Lerp(startPosition, endPosition, ratio),transform.position.y);
            ratio += Time.deltaTime / speed;
            if(ratio > 1)
            {
                jibExtend = false;
                ratio = 0;
                craneSensor.CraneMoveOn();
            }
        }
        if(jibContract)
        {
            transform.position = new Vector2(Mathf.Lerp(endPosition, startPosition, ratio), transform.position.y);
            ratio += Time.deltaTime / speed;
            if (ratio > 1)
            {
                jibContract = false;
                ratio = 0;
            }
        }
    }
    public void CanExtend()
    {
        jibExtend = true;
    }
}
