using System;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Scirpts
{
	public class StateMachine : MonoBehaviour
	{
		private enum PlayerStates
		{
			Spawning,
			AllowShootInput,
			Flying,
			Snapped,
			Death,
			Snapping,
			Won
		}

		[SerializeField] private Level _level;
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private float _shootMultiplier;
		[SerializeField] private Text _stateText;

		[SerializeField] private Transform _back;
		[SerializeField] private Transform _front;

		private ReactiveProperty<bool> _allowTrajectoryInput;
	
		private float _speed;
		private PlayerStates _state1 = PlayerStates.Spawning;

		private void Awake()
		{
			_allowTrajectoryInput = new ReactiveProperty<bool>(false);
		}

		private PlayerStates State
		{
			get { return _state1; }
			set
			{
				_state1 = value;
				_stateText.text = ""+ value;
			}
		}

		void Update ()
		{
			switch (State)
			{
				case PlayerStates.Spawning:
					HandleSpawning();
					break;

				case PlayerStates.AllowShootInput:
					HandleAllowShootInput();
					break;
				case PlayerStates.Flying:
					HandleFlying();
					break;
				case PlayerStates.Snapping:
					HandleSnapping();
					break;
				
				case PlayerStates.Snapped:
					HandleSnapped();
					break;
				case PlayerStates.Death:
					State = PlayerStates.Spawning;
					break;
				case PlayerStates.Won:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}	
		}

		private void HandleSnapped()
		{
			bool onGround = true;
			bool enemiesDead = true;
			
			if (onGround)
			{
				if (enemiesDead)
				{
					State = PlayerStates.Won;
				}
				else
				{
					State = PlayerStates.Death;
				}
			}
		}

		private void HandleSnapping()
		{
			if (Vector2.Distance(_back.position, _front.position) <= 1f)
			{
				Snap();
				State = PlayerStates.Snapped;
			}
		}

		private void Snap()
		{
			//SNAP those bitches
		}

		private void HandleFlying()
		{
			if (Input.anyKeyDown)
			{
				State = PlayerStates.Snapping;
			}
		}

		private void HandleAllowShootInput()
		{
			_allowTrajectoryInput.Value = true;
			if (Input.anyKeyDown)
			{
				_rigidbody2D.velocity = new Vector2(1f,1f) * _shootMultiplier;
				State = PlayerStates.Flying;
			}
		}

		private void HandleSpawning()
		{
			transform.position = _level.PlayerSpawn.position;
			_rigidbody2D.velocity = Vector2.zero;
			_rigidbody2D.inertia = 0;
			State = PlayerStates.AllowShootInput;
		}

		private void OnCollisionEnter2D(Collision2D other1)
		{
			if (State != PlayerStates.Snapped && State != PlayerStates.AllowShootInput)
			{
				State = PlayerStates.Death;
			}
		}
	}
}