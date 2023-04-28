using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private float _setTime = 0.0f;
    private float _time = 0.0f;
    public float Time => _time;

    // Start is called before the first frame update
    void Start()
    {
        _time = _setTime;        
    }

    // Update is called once per frame
    void Update()
    {
        CountDown();
    }

    /// <summary>
    /// カウントダウン処理
    /// </summary>
    private void CountDown()
    {
        _time--;
    }
}
