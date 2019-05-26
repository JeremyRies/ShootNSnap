using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
	private enum PlayerStates
	{
		Spawning,
		Moving,
		Falling,
		OnGround,
		StandingUp,
	}

	[SerializeField] private Transform _body;
	[SerializeField] private Transform _bottomAnchor;

	[SerializeField] private float _acceleration;
	[SerializeField] private float _criticalFallingSpeed;

	[SerializeField] private Text _debugStateText;
	
	private float _speed;
	

	[SerializeField] private Dictionary<PlayerStates,float> _durationForState;
	private Dictionary<PlayerStates,float> _currentTimeInState = new Dictionary<PlayerStates, float>();
	
	private PlayerStates _state = PlayerStates.Moving;
	private PlayerStates State
	{
		get { return _state;}
		set
		{
			_currentTimeInState[_state] = 0;
			_state = value;
		}
	}
	
	void Update ()
	{
		_debugStateText.text = State.ToString();
		_currentTimeInState[_state] += Time.deltaTime;
			
		switch (State)
		{
			case PlayerStates.Spawning:
				HandleSpawning();
				break;
			case PlayerStates.Moving:
				HandleMoving();
				break;
			case PlayerStates.Falling:
				HandleFalling();
				break;
			case PlayerStates.OnGround:
				HandleOnGround();
				break;
			case PlayerStates.StandingUp:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
	}

	private void HandleOnGround()
	{
		if (_currentTimeInState[PlayerStates.OnGround] > _durationForState[PlayerStates.OnGround])
		{
			State = PlayerStates.Moving;
		}
	}

	private void HandleSpawning()
	{
		//todo play falling animation
		State = PlayerStates.Moving;
	}

	private void HandleFalling()
	{
		if (_currentTimeInState[PlayerStates.Falling] > _durationForState[PlayerStates.Falling])
		{
			State = PlayerStates.OnGround;
		}
	}

	private void HandleMoving()
	{
		var xinput = Input.GetAxis("Horizontal");
        		
		_bottomAnchor.localRotation = Quaternion.AngleAxis(-xinput *50, new Vector3(0,0,1));

		_speed += xinput * _acceleration;
		_body.transform.position += Vector3.right * _speed;

		if (_speed > _criticalFallingSpeed)
		{
			State = PlayerStates.Falling;
		}
	}
}