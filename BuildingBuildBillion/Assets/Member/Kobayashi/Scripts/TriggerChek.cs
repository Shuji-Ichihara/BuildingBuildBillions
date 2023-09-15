using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TriggerChek : MonoBehaviour
{
    public UnityEvent<Collider2D> OnColliderStay;   //UnityEvent�̒�`�@����Collider2D
    public UnityEvent ExitEvent = new UnityEvent(); //Exit�悤�C�x���g

    /// <summary>
    /// �W���b�L�̏�Ɖ��̃u���b�N���m
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        // OnColliderStay�Ɋi�[���ꂽUnityEvent���Ăяo���A������collision��n���Ă���
        OnColliderStay.Invoke(collision);
    }
    /// <summary>
    /// ���m���痣�ꂽ���̏���
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�o�^�����C�x���g�𔭓�
        ExitEvent.Invoke();
    }
}
