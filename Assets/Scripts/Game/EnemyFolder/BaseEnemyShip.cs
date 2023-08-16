using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;


public class BaseEnemyShip : MonoBehaviour
{
    [HideInInspector] public PlayerShip _player;
    [HideInInspector] public Transform _myRoot;

    private Subject<MonoBehaviour> _putMe = new Subject<MonoBehaviour>();
    public IObservable<MonoBehaviour> PutMe => _putMe;
}
