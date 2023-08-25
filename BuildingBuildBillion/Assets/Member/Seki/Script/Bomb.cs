using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log(this.transform.localPosition);
            //BombEvent();
        }*/
        if (GameManager.Instance.IsEndedGame == true)
        {
            this.enabled = false;
        }
    }

    void BombEvent()
    {//ここで爆発
        StartCoroutine(BombCor());
        //_bombCollider.radius = 5;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        //_bombCollider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //_animator.SetBool("BombBool", true);
    }

    public void AnimStart()
    {
        _animator.SetBool("BombBool", true);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bill") || col.CompareTag("Bill2"))
        {
            //コライだー通り抜けたら飛ばす
            //ver2
            Vector3 hit;
            hit = col.ClosestPoint(this.transform.position);//trigger衝突位置


            hit = transform.InverseTransformPoint(hit);//触れたオブジェクトのローカル座標
                                                       //Debug.Log(hit);
                                                       //Debug.LogError(Mathf.Atan2(hit.y,hit.x)*Mathf.Rad2Deg);//角度だよ
            try
            {
                Rigidbody2D rigidbody2D = col.gameObject.GetComponent<Rigidbody2D>();
                if (rigidbody2D != null)
                {
                    rigidbody2D.AddForce(hit * power, ForceMode2D.Impulse);
                }

            }
            catch (System.Exception e)
            {
                throw e;
            }
            //吹っ飛ばす
        }
    }

    IEnumerator BombCor()
    {
        //爆発効果音入れるならココ!!
        SoundManager.Instance.PlaySE(SESoundData.SE.ExplosionBomb);

        _bombCollider.enabled = false;
        _bombCollider.enabled = true;
        _bombCollider.isTrigger = true;
        float count = 0;
        float time = 1f / bombSpeed;
        _rb.bodyType = RigidbodyType2D.Static;
        while (count < time)
        {
            _bombCollider.radius += Time.deltaTime * bombSpeed * bombRadius;
            count += Time.deltaTime;
            yield return null;
        }
        _bombCollider.enabled = false;
    }
}
