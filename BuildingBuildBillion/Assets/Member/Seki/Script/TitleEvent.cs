using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleEvent : MonoBehaviour
{

    [SerializeField]
    GameObject titleObj, startObj;

    Animator titleAnimator, startAnimator;

    //
    private bool isSceneMove = false;
    // Start is called before the first frame update
    void Start()
    {
        isSceneMove = false;
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
    {//コントローラーA押したときのヤツ
        //SceneMove.instance.MainGame();

        //
        SoundManager.Instance.StopBGM();
        //ボタンプッシュ効果音はココ!!
        SoundManager.Instance.PlaySE(SESoundData.SE.PressButton);
        startAnimator.SetInteger("ButtonINT", 1);
    }

    void PullA()
    {
        if (isSceneMove == false)
        { 
            //コントローラーA離したときのヤツ
            SceneMove.instance.MainGame();
            startAnimator.SetInteger("ButtonINT", 0);
            Debug.Log("遷移した");
            isSceneMove = true;
        }
    }



    void B()
    {//コントローラーB押したときのヤツ
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
            Debug.Log("このシーンにタイトルは無い");
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