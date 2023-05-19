using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public static SceneMove instance;
    [SerializeField]
    private string _titleSceneName, _mainGameSceneName, _resultSceneName;

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

    public void Title()
    {
        SceneManager.LoadScene(_titleSceneName);
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
