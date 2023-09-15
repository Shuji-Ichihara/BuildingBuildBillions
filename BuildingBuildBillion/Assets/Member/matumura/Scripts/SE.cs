using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SE : MonoBehaviour
{

    //�Q�[�����Ŗ炷SE
    [SerializeField, Header("�Q�[���V�[���Ŏg��SE")]public AudioClip sound1;
    [SerializeField, Header("�Q�[���V�[���Ŏg��SE")] public AudioClip sound2;


    //�V�[���J�ڎ��ɖ炷SE
    [SerializeField, Header("�^�C�g������J�ڎ��ɖ�SE")] public AudioClip StartSound;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //AudioSource�̎Q��
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //input.GetKey�̕�����Ή������������ɏ��������ĉ�����
        if (Input.GetKey(KeyCode.A))
        {
            audioSource.PlayOneShot(sound1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            audioSource.PlayOneShot(sound2);
        }

    }

    //�{�^������������SE�Đ���ɑJ��
    public void StartOnClick()
    {
        audioSource.PlayOneShot(StartSound);
        //�V�[�����͍��킹�Ƃ��Ă�������
        SceneManager.LoadScene("MainGame");
    }
}