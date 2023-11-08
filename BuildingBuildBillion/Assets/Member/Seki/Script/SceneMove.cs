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
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.TitleBGM);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Title();
        }
        QuitApplication();

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

    public void TitleMove()
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

    /// <summary>
    /// ÉAÉvÉäèIóπ
    /// </summary>
    private void QuitApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();        
#endif
        }
    }
}
