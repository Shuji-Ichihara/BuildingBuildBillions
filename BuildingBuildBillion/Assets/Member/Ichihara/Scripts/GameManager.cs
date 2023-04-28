using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private float _setTime = 0.0f;
    private float _countDownTime = 0.0f;
    public float CountDownTime => _countDownTime;

    // Start is called before the first frame update
    void Start()
    {
        _countDownTime = _setTime;        
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
        _countDownTime -= Time.deltaTime;
    }
}
