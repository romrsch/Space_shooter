using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// Сколько будет врагов и когда они оживут
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _minDelay = 2;
    [SerializeField] private float _maxDelay = 4;
    [SerializeField] private int _maxCountOneSpawn = 5;

    private float _timerDelay;
    private int _countOnePull;
    private SpawnManager _spawnManager;
    private CompositeDisposable _disposablesEnemy = new CompositeDisposable();
    private Coroutine _coroutine;

    private void Awake() 
    {
        _spawnManager = GetComponent<SpawnManager>();
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
                _countOnePull = Random.Range(1,_maxCountOneSpawn);
                _timerDelay = Random.Range(_minDelay, _maxDelay);
                for (int i = 0; i < _countOnePull; i++)
                { 
                    var hunter = _spawnManager.SpawnEnemy();
                    //if (ship != null) continue;
                    //var hunter = ship as Hunter;
                    
                    if (hunter != null)
                    {
                        hunter.Fire.Subscribe((param)=> Fire(param.Item1, param.Item2)).AddTo(_disposablesEnemy);
                    }
                    yield  return null;
                }
                _countOnePull = Random.Range(1, _maxCountOneSpawn);
            }
            yield return null;
        }
    }
    private void Fire(Transform tr, Bullet bullet)
    {
        Debug.LogError("Fire EC");
        _spawnManager.SpawnBullet(tr, bullet);
    }

   private void OnDisable()
    {
        if( _coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _disposablesEnemy.Dispose();
        _disposablesEnemy = null;

    }
 
}
