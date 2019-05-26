using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 
public class Level : MonoBehaviour
{
	public Transform PlayerSpawn;
}

public class StateMachine : MonoBehaviour
{
	private enum PlayerStates
	{
		Spawning,
		AllowShootInput,
		Flying,
		Snapped,
		Death
	}

	[SerializeField] private Level _level;

	[SerializeField] private Transform _body;

	[SerializeField] private Text _debugStateText;
	
	private float _speed;


	private PlayerStates State { get; set; } = PlayerStates.Spawning;

	void Update ()
	{

		switch (State)
		{
			case PlayerStates.Spawning:
				HandleSpawning();
				break;

			default:
				throw new ArgumentOutOfRangeException();
		}	
	}

	private void HandleSpawning()
	{
		transform.position = _level.PlayerSpawn.position;
	}

//	private void HandleFalling()
//	{
//		if (_currentTimeInState[PlayerStates.Falling] > _durationForState[PlayerStates.Falling])
//		{
//			State = PlayerStates.OnGround;
//		}
//	}

	private void HandleMoving()
	{

	}
}