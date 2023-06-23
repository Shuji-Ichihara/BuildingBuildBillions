using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class JadgementBar : MonoBehaviour
{
    /// <summary>
    /// 判定バーが建材と接触した時の勝敗判定
    /// </summary>
    /// <param name="other">接触したオブジェクトのコライダー</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        JadgementBarController.Instance.Objects.Add(other.gameObject);
        GameManager.Instance.IsPreviewedResult = true;
    }
}
