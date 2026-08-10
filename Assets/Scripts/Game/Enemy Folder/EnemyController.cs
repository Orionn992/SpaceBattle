using System.Collections;
using UniRx;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _minDelay = 2;
    [SerializeField] private float _maxDelay = 4;
    [SerializeField] private int _maxCounterOneSpawner = 5;
    private float _timerDelay;
    private int _countOnePull;
    private spawnManager _spawnManager;
    private CompositeDisposable _disposablesEnemy = new CompositeDisposable();
    private Coroutine _coroutine;

    private void Awake()
    {
        _spawnManager = GetComponent<spawnManager>();
        _timerDelay = Random.Range(_minDelay, _maxDelay);
    }
    private void OnEnable()
    {
        _disposablesEnemy = new CompositeDisposable();
        _coroutine = StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            _timerDelay -= Time.deltaTime;
            if (_timerDelay < 0)
            {
                _countOnePull = Random.Range(1, _maxCounterOneSpawner);
                _timerDelay = Random.Range(_minDelay, _maxDelay);
                for (int i = 0; i < _countOnePull; i++)
                {
                    var hunter = _spawnManager.SpawnEnemy();
                    if (hunter != null)
                    {
                        hunter.Fire.Subscribe((param) => Fire(param.Item1, param.Item2)).AddTo(_disposablesEnemy);
                    }
                    yield return null;
                }
                _countOnePull = Random.Range(1, _maxCounterOneSpawner);
            }
            yield return null;
        }
    }
    private void Fire(Transform tr, Bullet bullet)
    {
        _spawnManager.SpawnBullet(tr, bullet);
    }
    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _disposablesEnemy.Dispose();
        _disposablesEnemy = null;
    }
}