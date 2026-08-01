using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    MainMenu,
    Game
}
public class LevelManager : MonoBehaviour
{
    public LevelManager Instance;
    void Start()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        PlayScene(Scenes.MainMenu);
    }
    public static void PlayScene(Scenes sceneEnum)
    {
        SceneManager.LoadScene(sceneEnum.ToString());
    }
}
