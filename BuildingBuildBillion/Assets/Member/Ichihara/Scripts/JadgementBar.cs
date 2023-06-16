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
        Debug.Log($"aaaa");
        JadgementBarController.Instance.Objects.Add(other.gameObject);
        GameManager.Instance.IsPreviewedResult = true;

        /*
        //JadgementBarController.Instance.Jadge(other.gameObject.transform.position);
        bool winPlayer1 = other.gameObject.CompareTag("Bill");
        bool winPlayer2 = other.gameObject.CompareTag("Bill2");

        // 引き分け
        if (true == winPlayer1 && true == winPlayer2)
        {
            Debug.Log("引き分け");
            UIManager.Instance.DrawText.fontSize = 180.0f;
            UIManager.Instance.DrawText.text
                = "Draw.\nThank you for Playing!";
            UIManager.Instance.Player1ResultText.text
                = "";
            UIManager.Instance.Player2ResultText.text
                = "";
        }
        // Player1 の勝利
        else if (true == winPlayer1)
        {
            UIManager.Instance.Player1ResultText.text
                = UIManager.Instance.YouWon;
            UIManager.Instance.Player2ResultText.text
                = UIManager.Instance.YouLost;
        }
        // Player2 の勝利
        else if (true == winPlayer2)
        {
            UIManager.Instance.Player1ResultText.text
                = UIManager.Instance.YouLost;
            UIManager.Instance.Player2ResultText.text
                = UIManager.Instance.YouWon;
        }
        */
    }
}
