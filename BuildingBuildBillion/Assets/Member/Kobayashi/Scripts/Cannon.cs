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
    private bool _isOnece = false;

    private void Start()
    {
        _rg = this.gameObject.GetComponent<Rigidbody2D>();

    }
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _rg.isKinematic= false;
        }
        if (_rg.IsSleeping())
        {
            return;
        }
        else
        {
            _time -= Time.deltaTime;
            if (_time <= 0 && _isOnece ==false)
            {
                StartCoroutine(CannonFire());
                _isOnece = true;
            }
        }
    }
    /// <summary>
    /// �C�x���g�o�^�p�֐�
    /// </summary>
    public void EventRegistration()
    {
        StartCoroutine(CannonFire());
    }

    /// <summary>
    /// ��C�̍U�����\�b�h
    /// </summary>
    private IEnumerator CannonFire()
    {
        _rg.constraints = RigidbodyConstraints2D.FreezeRotation; //contrains�̃��[�e�[�V�����Œ�
        yield return new WaitForSeconds(_fireTime);//�w�肵�����ԑ҂�
        GameObject clone = Instantiate(_cannonBullet, _cannonMuzzle.transform.position, this.transform.rotation);//�e����
        clone.GetComponent<Rigidbody2D>().AddForce(_cannnonBulletImpact * _cannonMuzzle.transform.right, ForceMode2D.Impulse);//�e�ɏՌ���^���Ĕ���
        _rg.constraints = RigidbodyConstraints2D.None;//contrains�̃`�F�b�N�����ׂĊO��
        this.enabled = false;   //�X�N���v�g���A�N�e�B�u�ɂ���B
    }
}