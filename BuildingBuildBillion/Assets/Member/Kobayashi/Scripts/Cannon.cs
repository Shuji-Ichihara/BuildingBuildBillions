using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : NewBuildingcon
{
    [SerializeField, Header("���˂܂łɂ����鎞��")] private float _fireTime = 1; //���˂ɂ�����b��
    [SerializeField, Header("�e���˂̃C���p�N�g")] private float _cannnonBulletImpact;   //�e�̃X�s�[�h
    [SerializeField] private GameObject _cannonBullet;
    [SerializeField] private GameObject _cannonMuzzle;
    private Transform _cannonTransform;
    private Rigidbody2D _rg;

    private void Start()
    {
        _rg = this.gameObject.GetComponent<Rigidbody2D>();

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