using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timeText = null;

    // Start is called before the first frame update
    void Start()
    {
        _timeText.text = GameManager.Instance.Time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
