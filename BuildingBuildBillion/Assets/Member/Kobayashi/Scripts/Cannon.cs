using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField, Header("��C�̉�]�p�x")] private float _rotValue; //��C�̉�]�p�x��
    [SerializeField, Header("���˂܂łɂ����鎞��")] private float _fireTime; //���˂ɂ�����b��
    [SerializeField, Header("�e���˂̃C���p�N�g")] private float _cannnonBulletImpact;   //�e�̃X�s�[�h
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
    /// ��C�̉E��]���\�b�h
    /// </summary>
    public void CannonLeftRotation()
    {

        // ���[�J�����W����ɁA��]���擾
        Vector3 localangle = _cannonTransform.localEulerAngles;
        //���[�J�����W����ɂ���_rotValue����]
        localangle.z += _rotValue;
        _cannonTransform.localEulerAngles = localangle; // ��]�p�x��ݒ�
    }
    /// <summary>
    /// ��C�̍���]���\�b�h
    /// </summary>
    public void CannonRightRotation()
    {
        // ���[�J�����W����ɁA��]���擾
        Vector3 localangle = _cannonTransform.localEulerAngles;
        //���[�J�����W����ɂ���_rotValue����]
        localangle.z -= _rotValue;
        _cannonTransform.localEulerAngles = localangle; // ��]�p�x��ݒ�
    }
    /// <summary>
    /// ��C�̍U�����\�b�h
    /// </summary>
    public void CannonFire()
    {
        GameObject clone = Instantiate(_cannonBullet, _cannonMuzzle.transform.position, this.transform.rotation);

        //Rigidbody2D rg2d = clone.GetComponent<Rigidbody2D>();
        clone.GetComponent<Rigidbody2D>().AddForce(_cannnonBulletImpact * _cannonMuzzle.transform.right, ForceMode2D.Impulse);
    }

}
