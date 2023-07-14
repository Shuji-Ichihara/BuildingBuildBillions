using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEvent : MonoBehaviour
{

    [SerializeField]
    private GameObject _fadeObj;
    [SerializeField]
    SpriteRenderer _spriteRenderer;

    public delegate void FadeEventDelegate();
    public FadeEventDelegate fadeEventDelegate;

    float r, g, b, a = 0;
    [SerializeField]
    private float _fadeTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = _fadeObj.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            FadeOut();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            FadeIn();
        }
    }

    public void FadeOut()
    {
        a = 0;
        StartCoroutine(FadeOutCor());
    }

    public void FadeIn()
    {
        a = 1;
        StartCoroutine(FadeInCor());
    }
    IEnumerator FadeOutCor()
    {
        while(a<1)
        {
            a+=Time.deltaTime/_fadeTime;
            SetColor();
            yield return null;
        }
        fadeEventDelegate();
        FadeIn();
        //SceneMove.instance.MainGame();
    }
    IEnumerator FadeInCor()
    {
        while (a>0)
        {
            a -= Time.deltaTime / _fadeTime;
            SetColor();
            yield return null;
        }
    }

    void SetColor()
    {
        
        if(a>1)
        {
            a = 1;
        }
        else if(a<0)
        {
            a = 0;
        }
        _spriteRenderer.color = new Color(r,g,b,a);
    }
}
