using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class InputTest : MonoBehaviour
{
    public enum PlayerNum
    {
        Player1 =0,
        Player2 =1
    }

    public PlayerNum jittai;
    // �{�^���̉������
    private bool _isLeftPressed = false;    //LB�������ꂽ���H
    private bool _isRightPressed = false;   //RB�������ꂽ���H
    private bool _isHozirontalPressed = false;//���E�L�[�������ꂽ���H
    private Vector2 _inputMove = Vector2.zero; //�u���b�N�ړ��̐��l�擾
    private float restTime = 0; //���E�L�[�̒P�������邽�߂̐���

    private void Update()
    {
        switch (jittai)
        {
            case PlayerNum.Player1:
                //�v���C���[�P�̈ړ�����
                break;
            case PlayerNum.Player2:
                //�v���C���[�Q�̈ړ�����
                break;
        }
        restTime += Time.deltaTime;
        if (_isLeftPressed)
        {
            Debug.Log("hidarinaga");    //����������
        }
        if (_isRightPressed)
        {
            Debug.Log("miginaga");    //�E��������
        }
        //���E�P����
        if (_inputMove.x != 0 && restTime >= 0.25f && _isHozirontalPressed == true)
        {
            _isHozirontalPressed = false;
            //_inputMove.x,this.tra.pos.y,0;
            Debug.Log(_inputMove.x);  //�u���b�N���E�ړ�����
            restTime = 0;
        }
        //��������
        Debug.Log(_inputMove.y); //�������̏���
    }

    public void Rotate(InputAction.CallbackContext context) //�L�[���͂��󂯕t���邽�߂̊֐�
    {
        Debug.Log(context.phase);
        var y = context.control.name;   //�����ꂽ�L�[�̖��O���擾

        switch (context.phase)  //���͏�ԂŔ��f
        {
            case InputActionPhase.Performed:    //���͂���Ă�����

                if (y == "leftShoulder")    //���͂���Ă��Ė��O���������ɂ����Ă�����bool��true�ɂ���
                {
                    _isLeftPressed = true;
                }
                if (y == "rightShoulder")//���͂���Ă��Ė��O���������ɂ����Ă�����bool��true�ɂ���
                {
                    Debug.Log("migi");
                    _isRightPressed = true;
                }
                break;
            case InputActionPhase.Canceled:     //���͂��I��������bool�����ׂ�false�ɂ���
                _isLeftPressed = false;
                _isRightPressed = false;
                break;
        }
    }

    // ���E�ړ�
    public void OnHold(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>();
        _isHozirontalPressed = true;
    }
}

