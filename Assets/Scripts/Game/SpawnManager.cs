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
    [SerializeField] private Transform _poolEnemyBullet;

    public GameObject BulletPref { get => BulletPref1; set => BulletPref1 = value; }
    public GameObject BulletPref1 { get => _bulletPref; set => _bulletPref = value; }

    private List<Transform> _rootEnemyType = new List<Transform>();
    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
        Controller.Instance.Score.Value = 0; // обнуляем количество очков
        _playerShip = Controller.Instance._myShip;
        _playerShip.FireClick.Subscribe((_) => SpawnBullet());

        foreach (var enemy in _enemyPrefabs)
        {
            GameObject root = new GameObject("root" + enemy.name);
            root.transform.parent = _poolEnemyRoot;
            _rootEnemyType.Add(root.transform);

        }
    }

    public void SpawnBullet(Transform enemyTransform = null, Bullet enemyBullet = null) // выстрелы не только для корабля героя, но и для врагов
    {
        GameObject bullet;
        Controller.Instance.PlayAudioShot();
        if (enemyTransform != null && enemyBullet != null)
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
            bullet.transform.parent = _poolBulletMy;
            var position = enemyTransform.transform.position;
            bullet.transform.position = new Vector3(position.x, position.y - 1.2f, 0);
        }
        else
        {
            if (_poolBulletMy.childCount > 0)
            {
                bullet = _poolBulletMy.GetChild(0).gameObject;
            }
            else
            {
                bullet = Instantiate(_bulletPref);
                bullet.GetComponent<Bullet>().PutMe.Subscribe(PutObject).AddTo(_disposables);
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
            objBull.transform.parent = _poolBulletMy;
            objBull.gameObject.SetActive(false);
            return;
        }

        var objShip = mono as BaseEnemyShip;
        if (objShip != null) 
        {
           // Controller.Instance.Score.Value += objShip.CostPointerScore;
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
