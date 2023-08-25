using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Jack : MonoBehaviour
{
    [SerializeField] private List<Sprite> _jackSprite = new List<Sprite>();//ジャッキの画像保管用\
    [SerializeField] private float _maxAllowedAngleUp = 5f;
    [SerializeField] private float _maxAllowedAngleDown = 175f;
    private SpriteRenderer _mainSprite;//オブジェクトのSpriteRenderer参照用
    private Animator _jackAnim;
    private bool isOne = false;
    private bool isUp = false;
    private bool isDown = false;
   
    private void Awake()
    {
        _mainSprite = this.gameObject.GetComponent<SpriteRenderer>();//SpriterRenderer格納
        _jackAnim = this.gameObject.GetComponent<Animator>();
       _mainSprite.sprite = _jackSprite[0];
    }
    private void FixedUpdate()
    {
        if(isUp == true && isDown == true && isOne == false)
        {
            JackingStart();
            isOne= true;
        }

    }
    public void UpEventHandler(Collider2D collision)
    {
        Vector2 contactNormal = collision.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2)transform.position;

        // ブロックの法線ベクトルとの角度を計算
        float angle = Vector2.Angle(Vector2.up, contactNormal);

        // 角度が一定の範囲内にない場合は処理をスキップ
        if (angle > _maxAllowedAngleUp)
        {
            isUp = false;
            return;
        }
        // 角度が許容範囲内の場合の処理
        isUp = true;
    }
    public void DownEventHandler(Collider2D collision)
    {
        Vector2 contactNormal = collision.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2)transform.position;

        // ブロックの法線ベクトルとの角度を計算
        float angle = Vector2.Angle(Vector2.up, contactNormal);

        Debug.Log(angle);
        // 角度が一定の範囲内にない場合は処理をスキップ
        if (angle < _maxAllowedAngleDown)
        {
            isDown = false;
            return;
        }
        // 角度が許容範囲内の場合の処理
        isDown = true;
    }

    public void ExitUpEventHandler()
    {
        isUp = false;
    }
    public void ExitDownEventHandler()
    {
        isDown = false;
    }
    /// <summary>
    /// ジャッキが広がる処理
    /// </summary>
    public async void JackingStart()
    {
        await Task.Delay(500);
        _jackAnim.SetTrigger("JackActive");
        _mainSprite.sprite = _jackSprite[1];
        await Task.Delay(500);
        _mainSprite.sprite = _jackSprite[2];
    }
}
