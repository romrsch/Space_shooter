using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UniRx;
using System;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private float _speed = 14;
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

    private IEnumerator Move()
    {
        while (transform.position.y < _goTo)
        { 
            transform.position += new Vector3(0, Time.deltaTime * _speed, 0);
            yield return null;
        }
        _putMe.OnNext(this);
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
   
}
