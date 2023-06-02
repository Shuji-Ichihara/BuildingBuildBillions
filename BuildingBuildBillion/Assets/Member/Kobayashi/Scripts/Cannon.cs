using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField, Header("���˂܂łɂ����鎞��")] private float _fireTime = 1; //���˂ɂ�����b��
    [SerializeField, Header("�e���˂̃C���p�N�g")] private float _cannnonBulletImpact;   //�e�̃X�s�[�h
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
        // ���[�J�����W����ɁA��]���擾
        Vector3 localangle = _cannonTransform.localEulerAngles;
        //���[�J�����W����ɂ���_rotValue����]
        //�����������Ȃ���rigidbody��constraints����������Ă����R�������Ȃ����炲�����ʓ��������߂ɂ���B
        localangle.z += 0.0001f;
        _cannonTransform.localEulerAngles = localangle; // ��]�p�x��ݒ�
        _rg.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(_fireTime);
        GameObject clone = Instantiate(_cannonBullet, _cannonMuzzle.transform.position, this.transform.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(_cannnonBulletImpact * _cannonMuzzle.transform.right, ForceMode2D.Impulse);
        _rg.constraints = RigidbodyConstraints2D.None;
        this.enabled = false;
    }


}