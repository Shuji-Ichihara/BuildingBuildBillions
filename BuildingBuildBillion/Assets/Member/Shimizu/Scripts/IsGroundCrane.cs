using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGroundCrane : MonoBehaviour
{
    [SerializeField]
    CraneJib craneJib;
    private bool limit = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (limit)
        {
            limit = false;
            craneJib.CanTurn();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (limit)
        {
            limit = false;
            craneJib.CanTurn();
        }
    }
}
