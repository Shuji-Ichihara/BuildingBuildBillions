using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public IEnumerator DebugTest()
    {
        Debug.Log("ugoita");
        yield return new WaitForSeconds(3f);
        Debug.Log("3byoudattayo");
    }
    public void Hoge()
    {
        StartCoroutine(DebugTest());
    }
}
