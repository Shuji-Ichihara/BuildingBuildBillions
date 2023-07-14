using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneJib : NewBuildingcon
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
    [Header("ÉAÅ[ÉÄäpìx"),SerializeField]
    private float angle = 30f;
    [SerializeField]
    private GameObject jib2TurnPoint;
    [SerializeField]
    private GameObject jib;

    private void Start()
    {
        startPosition = jib.transform.localPosition.x;
        endPosition = jib.transform.localPosition.x + 4;
        craneSensor.GetComponent<Transform>();
    }
    private void Update()
    {
        //30ìx(ê›íËÇµÇΩäpìx)Ç…ã»Ç∞ÇÈ
        if (jibTurn)
        {
            jib2TurnPoint.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, angle), ratioRotation);
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
        //äpìxÇñﬂÇ∑
        if (jibReTurn)
        {
            jib2TurnPoint.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, angle), Quaternion.Euler(0, 0, 0), ratioRotation);
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
        //òrÇ™êLÇ—ÇÈ
        if (jibExtend)
        {
            jib.transform.localPosition = new Vector2(Mathf.Lerp(startPosition, endPosition, ratio), jib.transform.localPosition.y);
            ratio += Time.deltaTime / speed;
            if(ratio > 1.1f)
            {
                jibExtend = false;
                ratio = 0;
                craneSensor.CraneMoveOn();
            }
        }
        //òrÇ™èkÇﬁ
        if(jibContract)
        {
            jib.transform.localPosition = new Vector2(Mathf.Lerp(endPosition, startPosition + 1.5f, ratio), jib.transform.localPosition.y);
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
    /// <summary>
    /// ÉCÉxÉìÉgìoò^ópä÷êî
    /// </summary>
    public void EventRegistration()
    {
        CanTurn();
    }
}
