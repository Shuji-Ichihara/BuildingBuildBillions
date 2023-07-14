using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleEvent : MonoBehaviour
{

    [SerializeField]
    GameObject titleObj;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = titleObj.GetComponent<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        animator.SetBool("Anim", true);
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

    void A()
    {//コントローラーA押したときのヤツ
        SceneMove.instance.MainGame();
    }

    void B()
    {//コントローラーB押したときのヤツ
        SceneMove.instance.Tutorial();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator = titleObj.GetComponent<Animator>();
       
    }
}
