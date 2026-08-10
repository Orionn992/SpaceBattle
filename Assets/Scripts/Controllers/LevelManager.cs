using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes
{
    MainMenu,
    Game
}

public class LevelManager : MonoBehaviour
{
    private static float FadeSpeed = 0.02f;
    private static Color FadeTransparency = new Color(0, 0, 0, 0.04f);
    private static AsyncOperation _asyns;
    public static LevelManager Instance;
    public GameObject _faderObj;
    public Image _faderImg;

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
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    public static void PlayScene(Scenes sceneEnum)
    {
        Instance.LoadScene(sceneEnum.ToString());
    }
    private void LoadScene(string sceneName)
    {
        Instance.StartCoroutine(Load(sceneName));
        Instance.StartCoroutine(FadeOut(Instance._faderObj, Instance._faderImg));

    }
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Instance.StartCoroutine(FadeIn(Instance._faderObj, Instance._faderImg));
    }
    private static IEnumerator FadeOut(GameObject faderObject, Image fader)
    {
        faderObject.SetActive(true);
        while (fader.color.a < 1)
        {
            fader.color += FadeTransparency;
            yield return new WaitForSeconds(FadeSpeed);
        }
        ActivateScene();
    }
    private static IEnumerator FadeIn(GameObject faderObject, Image fader)
    {
        faderObject.SetActive(true);
        while (fader.color.a > 0)
        {
            fader.color -= FadeTransparency;
            yield return new WaitForSeconds(FadeSpeed);
        }
        faderObject.SetActive(false);
    }
    private static IEnumerator Load(string sceneName)
    {
        _asyns = SceneManager.LoadSceneAsync(sceneName);
        _asyns.allowSceneActivation = false;
        yield return _asyns;
    }
    private static void ActivateScene()
    {
        _asyns.allowSceneActivation = true;
    }
}