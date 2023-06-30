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
    private int bombRadius, bombSpeed,power=0;
    // Start is called before the first frame update
    void Start()
    {
        _bombCollider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(this.transform.localPosition);
            //BombEvent();
        }
    }

    void BombEvent()
    {
        StartCoroutine(BombCor());
        //_bombCollider.radius = 5;
        GetComponent<SpriteRenderer>().enabled = false;
        //_bombCollider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        _animator.SetBool("BombBool", true);
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //ver2
        Vector3 hit;
        hit = col.ClosestPoint(this.transform.position);//trigger衝突位置


        hit = transform.InverseTransformPoint(hit);//触れたオブジェクトのローカル座標
        //Debug.Log(hit);
        //Debug.LogError(Mathf.Atan2(hit.y,hit.x)*Mathf.Rad2Deg);//角度だよ

        col.gameObject.GetComponent<Rigidbody2D>().AddForce(hit*power, ForceMode2D.Impulse);//吹っ飛ばす
    }

    IEnumerator BombCor() 
    {
        _bombCollider.isTrigger = true;
        float count = 0;
        float time = 1f / bombSpeed;
        _rb.bodyType = RigidbodyType2D.Static;
        while(count<time)
        {
            _bombCollider.radius += Time.deltaTime*bombSpeed*bombRadius;
            count+=Time.deltaTime;
            yield return null;
        }
        _bombCollider.enabled = false;
    }
}
