using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public static SceneMove instance;
    [SerializeField]
    private string _titleSceneName,_tutorialScene, _mainGameSceneName, _resultSceneName;

    FadeEvent _fadeEvent;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(_fadeEvent==null)
        {
            _fadeEvent = GetComponent<FadeEvent>();
        }
       
        _fadeEvent.FadeIn();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Title();
        }
    }
    public void Title()
    {
        _fadeEvent.fadeEventDelegate += TitleMove;
        FadeOut();
    }
    public void Tutorial()
    {
        _fadeEvent.fadeEventDelegate -= TutorialMove;   
        FadeOut();
    }
    public void MainGame()
    {
        _fadeEvent.fadeEventDelegate += MainGameMove;
        FadeOut();
    }
    public void Result()
    {
        _fadeEvent.fadeEventDelegate = ResultMove;
       FadeOut();
    }

    void FadeOut() 
    {
        _fadeEvent.FadeOut();
    }

    void TitleMove()
    {
        SceneManager.LoadScene(_titleSceneName);
    }

    void TutorialMove()
    {
        SceneManager.LoadScene(_tutorialScene);
    }

    void MainGameMove()
    {
        SceneManager.LoadScene(_mainGameSceneName);
    }
    void ResultMove()
    {
        SceneManager.LoadScene(_resultSceneName);
    }
}
