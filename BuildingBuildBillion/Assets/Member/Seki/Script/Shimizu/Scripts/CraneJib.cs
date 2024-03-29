using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneJib : NewBuildingcon
{
    private float startPosition = 15;
    
    private float ratio = 0;

    private bool jibExtend = false;
    private bool jibContract = false;
    [SerializeField]
    CraneSensor2 craneSensor;
    [SerializeField]
    private SpriteRenderer armSprite;
    [Header("アームが横に伸びるスピード"),SerializeField]
    private float speed = 2f;
    [Header("アームが横に伸びるMAX長さ(90が限界"), SerializeField]
    private float endPosition = 19;

    private void Start()
    {

        Debug.Log(armSprite.size);
        craneSensor.GetComponent<Transform>();
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    CanExtend();
        //}
        //腕が伸びる
        if (jibExtend)
        {
            //
            armSprite.size = new Vector2(Mathf.Lerp(startPosition, endPosition, ratio), armSprite.size.y);
            ratio += Time.deltaTime / speed;
            if (ratio > 1.1f)
            {
                jibExtend = false;
                ratio = 0;
                craneSensor.CraneMoveOn();
            }
            //
            //Debug.Log(armSprite.size);
        }
        //腕が縮む
        if(jibContract)
        {
            //
            armSprite.size = new Vector2(Mathf.Lerp(endPosition, startPosition + 1.5f, ratio), armSprite.size.y);
            //
            ratio += Time.deltaTime / speed;
            if (ratio > 1.1f)
            {
                jibContract = false;
                ratio = 0;
            }
            
        }
    }
    private void CanExtend()
    {
        jibExtend = true;
        craneSensor.widthExtend = true;
    }
    public void CanContract()
    {
        jibContract= true;
        craneSensor.widthContract = true;
    }
    public float ExtendSpeed()
    {
        return speed;
    }
    public float ExtendDistance()
    {
        return endPosition-startPosition;
    }
    /// <summary>
    /// イベント登録用関数
    /// </summary>
    public void EventRegistration()
    {
        CanExtend();
    }
}
