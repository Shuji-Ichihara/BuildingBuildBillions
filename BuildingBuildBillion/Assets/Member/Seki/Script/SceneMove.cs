using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public static SceneMove instance;
    [SerializeField]
    private string _titleSceneName,_tutorialScene, _mainGameSceneName, _resultSceneName;

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
        SceneManager.LoadScene(_titleSceneName);
    }
    public void Tutorial()
    {
        SceneManager.LoadScene(_tutorialScene);
    }
    public void MainGame()
    {
        SceneManager.LoadScene(_mainGameSceneName);
    }
    public void Result()
    {
        SceneManager.LoadScene(_resultSceneName);
    }
}
