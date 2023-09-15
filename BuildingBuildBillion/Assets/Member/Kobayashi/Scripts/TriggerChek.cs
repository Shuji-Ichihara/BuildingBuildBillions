using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TriggerChek : MonoBehaviour
{
    public UnityEvent<Collider2D> OnColliderStay;   //UnityEventの定義　引数Collider2D
    public UnityEvent ExitEvent = new UnityEvent(); //Exitようイベント

    /// <summary>
    /// ジャッキの上と下のブロック検知
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        // OnColliderStayに格納されたUnityEventを呼び出し、引数にcollisionを渡している
        OnColliderStay.Invoke(collision);
    }
    /// <summary>
    /// 検知から離れた時の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //登録したイベントを発動
        ExitEvent.Invoke();
    }
}
