using UnityEngine;
using UnityEngine.SceneManagement;

public class BgmManager : MonoBehaviour
{
    public static BgmManager instance;

    [System.Serializable]
    public class SceneBgm
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    public SceneBgm[] sceneBgms;
    private AudioSource bgmSource;
    private string currentScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayCurrentSceneBGM();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayCurrentSceneBGM();
    }

    private void PlayCurrentSceneBGM()
    {
        Scene currentActiveScene = SceneManager.GetActiveScene();
        string sceneName = currentActiveScene.name;

        for (int i = 0; i < sceneBgms.Length; i++)
        {
            if (sceneBgms[i].sceneName == sceneName)
            {
                PlayBGM(sceneBgms[i].bgmClip);
                break;
            }
        }
    }

    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip == null) return;

        if (bgmSource.clip != bgmClip)
        {
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }
}