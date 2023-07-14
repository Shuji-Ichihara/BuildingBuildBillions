using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : NewBuildingcon
{
    [SerializeField, Header("発射までにかかる時間")] private float _fireTime = 1; //発射にかかる秒数
    [SerializeField, Header("弾発射のインパクト")] private float _cannnonBulletImpact;   //弾のスピード
    [SerializeField] private GameObject _cannonBullet;
    [SerializeField] private GameObject _cannonMuzzle;
    private Transform _cannonTransform;
    private Rigidbody2D _rg;

    private void Start()
    {
        _rg = this.gameObject.GetComponent<Rigidbody2D>();

    }
    /// <summary>
    /// イベント登録用関数
    /// </summary>
    public void EventRegistration()
    {
        StartCoroutine(CannonFire());
    }

    /// <summary>
    /// 大砲の攻撃メソッド
    /// </summary>
    private IEnumerator CannonFire()
    {
        _rg.constraints = RigidbodyConstraints2D.FreezeRotation; //contrainsのローテーション固定
        yield return new WaitForSeconds(_fireTime);//指定した時間待つ
        GameObject clone = Instantiate(_cannonBullet, _cannonMuzzle.transform.position, this.transform.rotation);//弾生成
        clone.GetComponent<Rigidbody2D>().AddForce(_cannnonBulletImpact * _cannonMuzzle.transform.right, ForceMode2D.Impulse);//弾に衝撃を与えて発射
        _rg.constraints = RigidbodyConstraints2D.None;//contrainsのチェックをすべて外す
        this.enabled = false;   //スクリプトを非アクティブにする。
    }
}