using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField]
    private TextMeshProUGUI _timeText = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _timeText.text = string.Format("{0:#}", GameManager.Instance.CountDownTime);
    }
}
