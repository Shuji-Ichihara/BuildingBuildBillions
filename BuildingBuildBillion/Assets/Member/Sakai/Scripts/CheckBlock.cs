using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock : MonoBehaviour
{
    NewBuildingcon bill;
    SpriteGlowEffect glow;

    void Start()
    {
        bill = GetComponentInParent<NewBuildingcon>();
        glow = GetComponentInParent<SpriteGlowEffect>();

    }

    private void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("stage"))
        {
            bill.Stop = true;
            glow.GlowOff = true;
            bill.isopareton = false;
        }
        if (collision.gameObject.CompareTag("Bill"))
        {
            bill.BuildingStop = true;
            glow.GlowOff = true;
            bill.isopareton = false;
        }
        if (collision.gameObject.CompareTag("Bill2"))
        {
            bill.BuildingStop = true;
            glow.GlowOff = true;
            bill.isopareton = false;
        }
    }

}
