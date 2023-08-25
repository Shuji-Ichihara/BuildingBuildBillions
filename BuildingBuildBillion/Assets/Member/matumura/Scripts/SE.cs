using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SE : MonoBehaviour
{

    //ゲーム中で鳴らすSE
    [SerializeField, Header("ゲームシーンで使うSE")]public AudioClip sound1;
    [SerializeField, Header("ゲームシーンで使うSE")] public AudioClip sound2;


    //シーン遷移時に鳴らすSE
    [SerializeField, Header("タイトルから遷移時に鳴るSE")] public AudioClip StartSound;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //AudioSourceの参照
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //input.GetKeyの部分を対応した条件式に書き換えて下さい
        if (Input.GetKey(KeyCode.A))
        {
            audioSource.PlayOneShot(sound1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            audioSource.PlayOneShot(sound2);
        }

    }

    //ボタンを押したらSE再生後に遷移
    public void StartOnClick()
    {
        audioSource.PlayOneShot(StartSound);
        //シーン名は合わせといてください
        SceneManager.LoadScene("MainGame");
    }
}