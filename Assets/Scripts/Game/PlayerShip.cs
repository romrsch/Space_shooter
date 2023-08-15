using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UniRx;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
	[SerializeField] private float _speed = 15;
	[SerializeField] private float _coolDown = 0.1f;
	public int _maxHealth = 100;
	[SerializeField] private float _shipRollEuler = 45;
	[SerializeField] private float _shipRollSpeed = 80;
	[SerializeField] private float _smothness = 1.2f;
	
	private Subject<Unit> _fireClick = new Subject<Unit>();
	public IObservable<Unit> FireClick => _fireClick;
	
	
	private Rigidbody2D _rigidbody;
	private float _coolDownCurrent = 10;
	private MeshRenderer _mR;
	private	Vector3 _sizeWorldShip;
	private Controller _controller;
	
	//public ReactiveProperty<int> _health = new ReactiveProperty<int>();
	[HideInInspector] public ReactiveProperty<int> _health = new ReactiveProperty<int>();
	
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_mR = GetComponent<MeshRenderer>();
		_controller = Controller.Instance;
		_controller._myShip = this;
		_sizeWorldShip = _mR.bounds.extents;
	}
	
	private void Start()
	{
			_controller.UpdateCameraSettings();
			_health.Value = _maxHealth;
	}
	
	private void Update()
	{
		UpdateKey();
		FireButtonClick();
	}

	private void FireButtonClick()
	{
		if (Input.GetMouseButton(0))
		{
			if(_coolDownCurrent >= _coolDown)
			{
				_coolDownCurrent = 0;
				_fireClick.OnNext(Unit.Default);
			}
		}
		if(_coolDownCurrent < _coolDown)
		{
			_coolDownCurrent += Time.deltaTime;
		}
	}


	private void UpdateKey()
	{
		float moveHor = Input.GetAxis("Horizontal");
		float moveVert = Input.GetAxis("Vertical");
		
		_rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, new Vector2(moveHor * _speed * 1.2f, moveVert * _speed), _smothness );
		 transform.position = CheckBoardWorld();
	}
	
	private Vector3 CheckBoardWorld()
    { 
        var pos = transform.position;
        var x = pos.x;
        var y = pos.y;

		 // clamp огнаничивает значение между макси и мин, а первый аргумент мы передаем
        x = Mathf.Clamp(x, _controller.LeftDownPoint.x + _sizeWorldShip.x, _controller.RightDownPoint.x - _sizeWorldShip.x);
        y = Mathf.Clamp(y, _controller.LeftDownPoint.y + _sizeWorldShip.y, _controller.LeftUpPoint.y - _sizeWorldShip.y);

        return new Vector3(x, y, 0);
    }
	
}