using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class BGMStop : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;

    

    

    public void OnClickStop()
    {
        // ��~���܂�
        AudioSource.Stop();
    }
}