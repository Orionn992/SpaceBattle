using UnityEngine;
using System;
using UniRx;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;
using Random = UnityEngine.Random;

public class spawnManager : MonoBehaviour
{
    private PlayerShip _playerShip;
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform _poolEnemyBullet;
    [SerializeField] private Transform _poolBulletMy;
    [SerializeField] private List<GameObject> _enemyPrefabs = new List<GameObject>();
    [SerializeField] private Transform _poolEnemyRoot;
    private List<Transform> _rootEnemyType = new List<Transform>();
    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
        Controller.Instance.Score.Value = 0;
        _playerShip = Controller.Instance._myShip;
        _playerShip.FireClick.Subscribe((_) => SpawnBullet());

        foreach (var enemy in _enemyPrefabs)
        {
            GameObject root = new GameObject("root" + enemy.name);
            root.transform.parent = _poolEnemyRoot;
            _rootEnemyType.Add(root.transform);
        }
    }

    public void SpawnBullet(Transform enemyTransform = null, Bullet enemyBullet = null)
    {
        GameObject bullet;

        Controller.Instance.PlayAudioShot();
        if (enemyBullet != null && enemyTransform != null)
        {
            if (_poolEnemyBullet.childCount > 0)
            {
                bullet = _poolEnemyBullet.GetChild(0).gameObject;
            }
            else
            {
                bullet = Instantiate(enemyBullet).gameObject;
                var bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.PutMe.Subscribe(PutObject).AddTo(_disposables);
            }
            bullet.transform.parent = transform;
            var position = enemyTransform.transform.position;
            bullet.transform.position = new Vector3(position.x, position.y -1.2f, 0);
        }
        else
        {


            if (_poolBulletMy.childCount > 0)
            {
                bullet = _poolBulletMy.GetChild(0).gameObject;
                bullet.transform.parent = transform;
            }
            else
            {
                bullet = Instantiate(_bulletPref);
                bullet.GetComponent<Bullet>().PutMe.Subscribe(PutObject).AddTo(bullet);
                bullet.transform.parent = _poolBulletMy;
                bullet.gameObject.SetActive(false);
            }
            bullet.transform.parent = transform;
            var pos = _playerShip.transform.position;
            bullet.transform.position = new Vector3(pos.x, pos.y + 1.2f, 0);
        }
        
        bullet.gameObject.SetActive(true);
    }

    public Hunter SpawnEnemy()
    {
        var controller = Controller.Instance;
        GameObject ship;
        int type = Random.Range(0, _enemyPrefabs.Count);
        var pool = _rootEnemyType[type];
        if (pool.childCount > 0)
        {
            ship = pool.GetChild(0).gameObject;
            ship.transform.parent = pool;
        }
        else
        {
            ship = Instantiate(_enemyPrefabs[type]);
            var enemyShip = ship.GetComponent<BaseEnemyShip>();
            enemyShip.PutMe.Subscribe(PutObject).AddTo(enemyShip);
            enemyShip._myRoot = pool;
            enemyShip._player = _playerShip;
            ship.transform.parent = pool;
            ship.SetActive(false);
        }
        ship.transform.parent = _poolEnemyRoot;
        var height = controller.RighUpPoint.y + 2;
        Vector3 spawnPos = new Vector3(
            Random.Range(controller.LeftUpPoint.x + 0.5f, controller.RighUpPoint.x - 0.5f),
            height,
            0
        );
        ship.transform.position = spawnPos;
        ship.SetActive(true);
        return ship.GetComponent<Hunter>();
    }

    private void PutObject(MonoBehaviour mono)
    {
        var objBull = mono as Bullet;
        if (objBull != null)
        {
            if (objBull._isEnemy)
            {
                objBull.transform.parent = _poolEnemyBullet;
            }
            else
            {
                objBull.transform.parent = _poolBulletMy;
            }
                objBull.gameObject.SetActive(false);
                return;
        }
        var objShip = mono as BaseEnemyShip;
        if (objShip != null)
        {
            objShip.transform.parent = objShip._myRoot;
            objShip.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        _disposables = new CompositeDisposable();
    }

    private void OnDisable()
    {
        _disposables.Dispose();
        _disposables = null;
    }
}