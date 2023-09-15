using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("BgmTest1");
        SceneManager.LoadScene("BgmTest2");    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
