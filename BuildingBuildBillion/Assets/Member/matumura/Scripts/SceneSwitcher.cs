using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToNewScene(string Game)
    {
        SceneManager.LoadScene(Game);
    }
}