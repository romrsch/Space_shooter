using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _minDelay = 2;
    [SerializeField] private float _maxDelay = 4;
    [SerializeField] private int _maxCountOneSpawn = 5;

    private float _timerDelay;
    private int _countOnePull;
    private SpawnManager _spawnManager;
    private CompositeDisposable _disposablesEnemy = new CompositeDisposable();

    private void Awake() 
    {
        _spawnManager = GetComponent<SpawnManager>();
        _timerDelay = Random.Range(_minDelay, _maxDelay);
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
                    var ship = _spawnManager.SpawnEnemy();
                }
            }
            yield return null;
        }
    }
}
