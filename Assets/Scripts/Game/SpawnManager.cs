using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System;
using UniRx;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{

    private PlayerShip _playerShip;

    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform _poolBulletMy;
    [SerializeField] private List<GameObject> _enemyPrefabs = new List<GameObject>();// enemy Ships
    [SerializeField] private Transform _poolEnemyRoot;

    public GameObject BulletPref { get => BulletPref1; set => BulletPref1 = value; }
    public GameObject BulletPref1 { get => _bulletPref; set => _bulletPref = value; }

    private List<Transform> _rootEnemyType = new List<Transform>();
    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
        _playerShip = Controller.Instance._myShip;
        _playerShip.FireClick.Subscribe((_) => SpawnBullet());

        foreach (var enemy in _enemyPrefabs)
        {
            GameObject root = new GameObject("root" + enemy.name);
            root.transform.parent = _poolEnemyRoot;
            _rootEnemyType.Add(root.transform);

        }
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

    public BaseEnemyShip SpawnEnemy()
    {
        var controller = Controller.Instance;
        GameObject ship;
        int type = Random.Range(0, _enemyPrefabs.Count);
        var pool = _rootEnemyType[type];

        if (pool.childCount > 0)
        {
            ship = pool.GetChild(0).gameObject;
        }
        else
        {
            ship = Instantiate(_enemyPrefabs[type]);
            var enemyShip = ship.GetComponent<BaseEnemyShip>();
            enemyShip.PutMe.Subscribe(PutObject).AddTo(_disposables);
            enemyShip._myRoot = pool;
            enemyShip._player = _playerShip;
        }
        ship.transform.parent = _poolEnemyRoot;
        var height = controller.RightUpPoint.y + 2;

        Vector3 spawnPos = new Vector3(Random.Range(controller.LeftUpPoint.x + 0.5f, controller.RightUpPoint.x - 0.5f), height, 0);

        ship.transform.position = spawnPos;
        ship.SetActive(true);

        return ship.GetComponent<BaseEnemyShip>();


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

    private void OnDisable() 
    {
        _disposables.Dispose();
    }

}
