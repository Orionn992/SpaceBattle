using UnityEngine;
using System;
using UniRx;

public class spawnManager : MonoBehaviour
{
    private PlayerShip _playerShip;
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform _poolBulletMy;
    private void Start()
    {
        _playerShip = Controller.Instance._myShip;
        _playerShip.FireClick.Subscribe((_) => SpawnBullet());
    }
    private void SpawnBullet()
    {
        GameObject bullet;
        if (_poolBulletMy.childCount > 0)
        {
            bullet = _poolBulletMy.GetChild(0).gameObject;
        }
        else
        {
            bullet = Instantiate(_bulletPref);
            bullet.GetComponent<Bullet>().PutMe.Subscribe(PutObject);
        }
        bullet.transform.parent = transform;
        var pos = _playerShip.transform.position;
        bullet.transform.position = new Vector3(pos.x, pos.y + 1.2f, 0);
        bullet.gameObject.SetActive(true);
    }
    private void PutObject(MonoBehaviour mono)
    {
        var objBull = mono as Bullet;
        if (objBull != null)
        {
            objBull.transform.parent = _poolBulletMy;
        }
        objBull.gameObject.SetActive(false);
    }
}
