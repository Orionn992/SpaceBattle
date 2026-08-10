using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    MainMenu,
    Game
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        PlayScene(Scenes.MainMenu);
    }
    
    public static void PlayScene(Scenes sceneEnum)
    {
        SceneManager.LoadScene(sceneEnum.ToString());
    }
}