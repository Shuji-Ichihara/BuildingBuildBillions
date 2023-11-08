using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    CircleCollider2D _bombCollider;
    Rigidbody2D _rb;
    Animator _animator;

    [SerializeField]
    GameObject colObj;
    [SerializeField, Header("ボムの爆発の範囲")]
    private int bombRadius = 0;
    [SerializeField, Header("ボムの爆発の広がるスピード")]
    private int bombSpeed = 0;
    [SerializeField, Header("ボムの吹っ飛ばすパワー")]
    private int power = 0;

    // Start is called before the first frame update
    void Start()
    {
        _bombCollider = colObj.GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            BombEvent();
        }
        */
    }

    void BombEvent()
    {//アニメーションから呼び出し

        StartCoroutine(BombCor());
        GetComponent<SpriteRenderer>().enabled = false;
    }


    public void AnimStart()
    {
        _animator.SetBool("BombBool", true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {//コライダー通り抜けたら飛ばす

        if (col.gameObject.GetComponent<NewBuildingcon>() != null)
        {
            if (col.gameObject.GetComponent<NewBuildingcon>().isopareton == false)
            {//建材が操作中かどうかの判断

                //吹っ飛ばす処理
                Vector3 hit;
                hit = col.ClosestPoint(this.transform.position);//trigger衝突位置
                hit = transform.InverseTransformPoint(hit);//触れたオブジェクトのローカル座標
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(hit * power, ForceMode2D.Impulse);
                SoundManager.Instance.PlaySE(SESoundData.SE.ExplosionBomb);
            }
        }
    }

    IEnumerator BombCor()
    {//爆発の一連の処理

        _bombCollider.enabled = false;
        _bombCollider.enabled = true;
        _bombCollider.isTrigger = true;

        float count = 0;
        float time = 1f / bombSpeed;

        _rb.bodyType = RigidbodyType2D.Static;

        while (count < time)
        {
            if (GameManager.Instance.IsEndedGame == true) 
            { 
                enabled = false;
                break;
            }
            _bombCollider.radius += Time.deltaTime * bombSpeed * bombRadius;
            count += Time.deltaTime;
            yield return null;
        }
        _bombCollider.enabled = false;
    }
}
