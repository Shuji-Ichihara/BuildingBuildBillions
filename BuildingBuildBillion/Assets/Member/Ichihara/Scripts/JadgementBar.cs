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
        JadgementBarController.Instance.Jadge(other.gameObject.transform.position);
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GameManager.Instance.IsPreviewedResult = true;
        if(true == other.gameObject.CompareTag("Bill"))
        {
            UIManager.Instance.Player1ResultText.text
                = UIManager.Instance.YouWon;
            UIManager.Instance.Player2ResultText.text
                = UIManager.Instance.YouLost;
        }
        else if(true == other.gameObject.CompareTag("Bill2"))
        {
            UIManager.Instance.Player1ResultText.text
                = UIManager.Instance.YouLost;
            UIManager.Instance.Player2ResultText.text
                = UIManager.Instance.YouWon;
        }
    }
}
