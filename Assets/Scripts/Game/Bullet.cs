using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UniRx;
using System;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private float _speed = 14;
    [SerializeField] private GameObject _destroyEffect;

    public int _damage = 3;

    private Subject<MonoBehaviour> _putMe = new Subject<MonoBehaviour>();

    public IObservable<MonoBehaviour> PutMe => _putMe;
    private float _goTo;
    public bool _isEnemy;

    private void OnEnable()
    {
        var controller = Controller.Instance;
        _goTo = controller.LeftUpPoint.y + 2;
        StartCoroutine(Move());
    }

    private IEnumerator Move() // куда двигается пуля
    {
        if (_isEnemy) // если пуля вражеская, то двигаем её вниз
        {
            while (transform.position.y > -_goTo) // пуля долетает до нижнего края -goTo
            {
                transform.position -= new Vector3(0, Time.deltaTime * _speed, 0);
                yield return null;
            }
        }
        else // иначе пуля - наша, двигаем её вверх
        {
            while (transform.position.y < _goTo)
            {
                transform.position += new Vector3(0, Time.deltaTime * _speed, 0);
                yield return null;
            }
        }
        _putMe.OnNext(this);
    }

    public void HitMe()
    {
        _putMe.OnNext(this);
        var pos = transform.position; // получаем позицию пули
        Instantiate(_destroyEffect, new(pos.x, pos.y, -2), transform.rotation);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
   
}
