using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField, Header("���˂܂łɂ����鎞��")] private float _fireTime; //���˂ɂ�����b��
    [SerializeField, Header("�e���˂̃C���p�N�g")] private float _cannnonBulletImpact;   //�e�̃X�s�[�h
    [SerializeField] private GameObject _cannonBullet;
    [SerializeField] private GameObject _cannonMuzzle;
    private Transform _cannonTransform;
    private Rigidbody2D _rg;
    private float rotValue = 1;//���ϐ�

    private void Start()
    {
        _rg = this.gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(CannonFire());//�����̌Ăԏꏊ�͔z�u������ł��ˁB
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
    /// ��C�̉�]���\�b�h
    /// ����:float RotValue
    /// �E��]�̎��͈����Ƀ}�C�i�X�����Ă��������B
    /// </summary>
    public void CannonRotation(float RotValue)
    {
        // ���[�J�����W����ɁA��]���擾
        Vector3 localangle = _cannonTransform.localEulerAngles;
        //���[�J�����W����ɂ���_rotValue����]
        localangle.z += RotValue;
        _cannonTransform.localEulerAngles = localangle; // ��]�p�x��ݒ�
    }
    /// <summary>
    /// ��C�̍U�����\�b�h
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
