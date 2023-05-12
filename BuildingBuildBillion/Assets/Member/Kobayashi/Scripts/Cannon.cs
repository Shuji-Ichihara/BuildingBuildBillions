using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField, Header("発射までにかかる時間")] private float _fireTime; //発射にかかる秒数
    [SerializeField, Header("弾発射のインパクト")] private float _cannnonBulletImpact;   //弾のスピード
    [SerializeField] private GameObject _cannonBullet;
    [SerializeField] private GameObject _cannonMuzzle;
    private Transform _cannonTransform;
    private Rigidbody2D _rg;
    private float rotValue = 1;//仮変数

    private void Start()
    {
        _rg = this.gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(CannonFire());//こいつの呼ぶ場所は配置したらでおね。
    }
    private void Update()
    {
        _cannonTransform = this.transform;
        Move(Input.GetAxis("D_Pad_H"), Input.GetAxis("D_Pad_V"));
    }
    private void Move(float hori, float ver)
    {
        if (Input.GetKey(KeyCode.D))
        {
            CannonRotation(-rotValue);
        }
        if (Input.GetKey(KeyCode.A))
        {
            CannonRotation(rotValue);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            _rg.constraints = RigidbodyConstraints2D.None;
        }

        if (Input.GetKeyDown("joystick button 4"))
        {
            Debug.Log("L"); //L?
        }
        if (Input.GetKeyDown("joystick button 5"))
        {
            Debug.Log("R");//R?
        }
        Debug.Log(hori);
        Debug.Log(ver);
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
        _rg.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(_fireTime);
        GameObject clone = Instantiate(_cannonBullet, _cannonMuzzle.transform.position, this.transform.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(_cannnonBulletImpact * _cannonMuzzle.transform.right, ForceMode2D.Impulse);
        _rg.constraints = RigidbodyConstraints2D.None;
    }


}
