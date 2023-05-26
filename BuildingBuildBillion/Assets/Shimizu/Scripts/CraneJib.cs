using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneJib : MonoBehaviour
{
    private float startPosition;
    private float endPosition;
    private float speed = 2f;
    private float ratio = 0;

    private float startRotation = 0;
    private float speedRotation = 2;
    private float ratioRotation = 0;

    private bool jibTurn = false;
    private bool jibReTurn = false;
    private bool jibExtend = false;
    private bool jibContract = false;
    [SerializeField]
    CraneSensor2 craneSensor;
    [Header("�A�[���p�x"),SerializeField]
    private float angle = 30f;
    private void Start()
    {
        startPosition = transform.localPosition.x;
        endPosition = transform.localPosition.x + 4.5f;
        craneSensor.GetComponent<Transform>();
    }
    private void Update()
    {
        //30�x(�ݒ肵���p�x)�ɋȂ���
        if (jibTurn)
        {
            craneSensor.transform.parent.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -angle), ratioRotation);
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, angle), ratioRotation);
            ratioRotation += Time.deltaTime / speedRotation;
            if (ratioRotation > 1.1f)
            {
                jibTurn = false;
                ratioRotation = 0;
                CanExtend();
            }
        }
        //�p�x��߂�
        if (jibReTurn)
        {
            craneSensor.transform.parent.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, -angle), Quaternion.Euler(0, 0, 0), ratioRotation);
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, angle), Quaternion.Euler(0, 0, 0), ratioRotation);
            ratioRotation += Time.deltaTime / speedRotation;
            if (ratioRotation > 1.1f)
            {
                jibReTurn = false;
                ratioRotation = 0;
                //CanContract();
            }
        }
        //Debug.Log(jibExtend);
        //�r���L�т�
        if (jibExtend)
        {
            transform.localPosition = new Vector2(Mathf.Lerp(startPosition, endPosition, ratio),transform.localPosition.y);
            ratio += Time.deltaTime / speed;
            if(ratio > 1.1f)
            {
                jibExtend = false;
                ratio = 0;
                craneSensor.CraneMoveOn();
            }
        }
        //�r���k��
        if(jibContract)
        {
            transform.localPosition = new Vector2(Mathf.Lerp(endPosition, startPosition+1.5f, ratio), transform.localPosition.y);
            ratio += Time.deltaTime / speed;
            if (ratio > 1.1f)
            {
                jibContract = false;
                ratio = 0;
                ReturnAngle();
            }
        }
    }
    public void CanTurn()
    {
        jibTurn = true;
    }
    public void ReturnAngle()
    {
        jibReTurn = true;
    }
    private void CanExtend()
    {
        jibExtend = true;
    }
    public void CanContract()
    {
        jibContract= true;
    }
    public void ExitContract()
    {
        jibContract = false;
    }
}
