using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleEvent : MonoBehaviour
{

    [SerializeField]
    GameObject titleObj,startObj;

    Animator titleAnimator,startAnimator;
    // Start is called before the first frame update
    void Start()
    {
        titleAnimator = titleObj.GetComponent<Animator>();
        startAnimator = startObj.GetComponent<Animator>();
        //SceneManager.sceneLoaded += OnSceneLoaded;
        titleAnimator.SetBool("Anim", true);
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("Anim", true);
        }*/
    }

    void PushA()
    {//�R���g���[���[A�������Ƃ��̃��c
        //SceneMove.instance.MainGame();

        //�{�^���v�b�V�����ʉ��̓R�R!!
        startAnimator.SetInteger("ButtonINT", 1);
    }

    void PullA()
    {//�R���g���[���[A�������Ƃ��̃��c
        SceneMove.instance.MainGame();
        startAnimator.SetInteger("ButtonINT", 0);
        Debug.Log("�J�ڂ���");
    }



    void B()
    {//�R���g���[���[B�������Ƃ��̃��c
        SceneMove.instance.Tutorial();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {/*
        if (titleObj != null && startObj != null)
        {
            titleAnimator = titleObj.GetComponent<Animator>();
            startAnimator = startObj.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("���̃V�[���Ƀ^�C�g���͖���");
        }*/
      
       
    }
    public void _Rotate(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {

            case InputActionPhase.Started:
                Debug.Log("osa");
                PushA();
                break;
            case InputActionPhase.Canceled:
                Debug.Log("ha");
                PullA();
                break;
        }

    }
}