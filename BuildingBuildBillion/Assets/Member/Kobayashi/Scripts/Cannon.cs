using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField, Header("大砲の回転角度")] private float _rotValue; //大砲の回転角度数
    [SerializeField, Header("発射までにかかる時間")] private float _fireTime; //発射にかかる秒数
    [SerializeField, Header("弾発射のインパクト")] private float _cannnonBulletImpact;   //弾のスピード
    [SerializeField] private GameObject _cannonBullet;
    [SerializeField] private GameObject _cannonMuzzle;
    private Transform _cannonTransform;

    private void Start()
    {
    }
    private void Update()
    {
        _cannonTransform = this.transform;
        if (Input.GetKey(KeyCode.D))
        {
            CannonRightRotation();
        }
        if (Input.GetKey(KeyCode.A))
        {
            CannonLeftRotation();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CannonFire();
        }
    }
    /// <summary>
    /// 大砲の右回転メソッド
    /// </summary>
    public void CannonLeftRotation()
    {

        // ローカル座標を基準に、回転を取得
        Vector3 localangle = _cannonTransform.localEulerAngles;
        //ローカル座標を基準にして_rotValue分回転
        localangle.z += _rotValue;
        _cannonTransform.localEulerAngles = localangle; // 回転角度を設定
    }
    /// <summary>
    /// 大砲の左回転メソッド
    /// </summary>
    public void CannonRightRotation()
    {
        // ローカル座標を基準に、回転を取得
        Vector3 localangle = _cannonTransform.localEulerAngles;
        //ローカル座標を基準にして_rotValue分回転
        localangle.z -= _rotValue;
        _cannonTransform.localEulerAngles = localangle; // 回転角度を設定
    }
    /// <summary>
    /// 大砲の攻撃メソッド
    /// </summary>
    public void CannonFire()
    {
        GameObject clone = Instantiate(_cannonBullet, _cannonMuzzle.transform.position, this.transform.rotation);

        //Rigidbody2D rg2d = clone.GetComponent<Rigidbody2D>();
        clone.GetComponent<Rigidbody2D>().AddForce(_cannnonBulletImpact * _cannonMuzzle.transform.right, ForceMode2D.Impulse);
    }

}
