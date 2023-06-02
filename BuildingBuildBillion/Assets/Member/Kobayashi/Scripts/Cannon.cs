using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField, Header("発射までにかかる時間")] private float _fireTime = 1; //発射にかかる秒数
    [SerializeField, Header("弾発射のインパクト")] private float _cannnonBulletImpact;   //弾のスピード
    [SerializeField] private GameObject _cannonBullet;
    [SerializeField] private GameObject _cannonMuzzle;
    private Transform _cannonTransform;
    private Rigidbody2D _rg;
    private float _time = 1f;
    private bool isOnece = false;

    private void Start()
    {
        _rg = this.gameObject.GetComponent<Rigidbody2D>();

    }
    private void FixedUpdate()
    {
        _cannonTransform = this.transform;
        if (_rg.IsSleeping())
        {
            return;
        }
        else
        {
            _time -= Time.deltaTime;
            if (_time <= 0 && isOnece ==false)
            {
                Debug.Log("hoge");
                StartCoroutine(CannonFire());
                isOnece = true;
            }
        }
    }

    /// <summary>
    /// 大砲の回転メソッド
    /// 引数:float RotValue
    /// 右回転の時は引数にマイナスをつけてください。
    /// </summary>
    public void CannonRotation(float RotValue)
    {
        // ローカル座標を基準に、回転を取得
        Vector3 localangle = _cannonTransform.localEulerAngles;
        //ローカル座標を基準にして_rotValue分回転
        localangle.z += RotValue;
        _cannonTransform.localEulerAngles = localangle; // 回転角度を設定
    }
    /// <summary>
    /// 大砲の攻撃メソッド
    /// </summary>
    public IEnumerator CannonFire()
    {
        // ローカル座標を基準に、回転を取得
        Vector3 localangle = _cannonTransform.localEulerAngles;
        //ローカル座標を基準にして_rotValue分回転
        //少し動かさないとrigidbodyのconstraintsが解除されても自由落下しないからごく少量動かすためにある。
        localangle.z += 0.0001f;
        _cannonTransform.localEulerAngles = localangle; // 回転角度を設定
        _rg.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(_fireTime);
        GameObject clone = Instantiate(_cannonBullet, _cannonMuzzle.transform.position, this.transform.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(_cannnonBulletImpact * _cannonMuzzle.transform.right, ForceMode2D.Impulse);
        _rg.constraints = RigidbodyConstraints2D.None;
        this.enabled = false;
    }


}