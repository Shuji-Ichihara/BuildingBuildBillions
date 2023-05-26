﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JadgementBar : MonoBehaviour
{
    // 判定バーが建材と接触した時の勝敗判定 
    private void OnCollisionEnter2D(Collision2D other)
    {
        JadgementBarControllerTest.Instance.Jadge(other.gameObject.transform.position);
    }
}
