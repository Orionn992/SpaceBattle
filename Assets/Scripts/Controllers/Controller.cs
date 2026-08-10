using UnityEngine;
using UniRx;
using System;

public class Controller : MonoBehaviour
{
    public HealthBonus _healthBonusPref;
    public int _procentHealthBonus = 30;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectSource;

    public ReactiveProperty<int> Score = new ReactiveProperty<int>();
    public AudioSource MusicSource => _musicSource;
    public AudioSource EffectSource => _effectSource;
    
    public static Controller Instance;
    public PlayerShip _myShip;
    
    private Vector3 _leftDownPoint;
    private Vector3 _rightDownPoint;
    private Vector3 _leftUpPoint;
    private Vector3 _righUpPoint;
    private Vector2 _centrCam;
    
    public Vector3 LeftDownPoint => _leftDownPoint;
    public Vector3 RightDownPoint => _rightDownPoint;
    public Vector3 LeftUpPoint => _leftUpPoint;
    public Vector3 RighUpPoint => _righUpPoint;
    public Vector2 CentrCam => _centrCam;
    private Subject<Unit> _gameOver = new Subject<Unit>();
    public IObservable<Unit> OnGameOver => _gameOver;
    
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

    public void UpdateCameraSettings()
    {
        var cameraMain = Camera.main;
        if (cameraMain != null)
        {
            float distance = Mathf.Abs(cameraMain.transform.position.z);
            _centrCam = cameraMain.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            _leftDownPoint = cameraMain.ScreenToWorldPoint(new Vector3(0, 0, distance));
            _rightDownPoint = cameraMain.ScreenToWorldPoint(new Vector3(cameraMain.pixelWidth, 0, distance));
            _leftUpPoint = cameraMain.ScreenToWorldPoint(new Vector3(0, cameraMain.pixelHeight, distance));
            _righUpPoint = cameraMain.ScreenToWorldPoint(new Vector3(cameraMain.pixelWidth, cameraMain.pixelHeight, distance));
        }
    }
    public void GameOver()
    {
        _gameOver.OnNext(Unit.Default);
    }
}