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
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("Anim", true);
        }
    }

    void A()
    {
        SceneMove.instance.MainGame();
    }

    void B() 
    {
        SceneMove.instance.Tutorial();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator = titleObj.GetComponent<Animator>();
       
    }
}
