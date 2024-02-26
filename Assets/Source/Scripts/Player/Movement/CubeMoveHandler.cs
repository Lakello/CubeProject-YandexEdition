using System;
using System.Collections;
using CubeProject.InputSystem;
using LeadTools.Extensions;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Player.Movement
{
	public class CubeMoveHandler : MonoBehaviour
	{
		[SerializeField] private float _rollSpeed;
		[SerializeField] private LayerMask _wallMask;
		[SerializeField] private AudioSource _audioSourceMove;

		private RollMover _roll;
		private BoxCollider _cubeCollider;
		private CubeStateHandler _stateHandler;
		private IInputHandler _inputHandler;
		private Coroutine _moveCoroutine;
		private bool _isBrake;

		public event Action StepEnded;

		public event Action<Vector3> StepStarted;

		[Inject]
		private void Inject(IInputHandler inputHandler, Cube cube)
		{
			_inputHandler = inputHandler;
			_inputHandler.MoveKeyChanged += OnMoving;
			_stateHandler = cube.ComponentsHolder.StateHandler;
			_cubeCollider = cube.ComponentsHolder.SelfCollider;
		}

		private void Awake() =>
			_roll = new RollMover(_audioSourceMove);

		public void Push(Vector3 direction)
		{
			if (_stateHandler.CurrentState != CubeState.Pushing)
			{
				return;
			}

			TryMove(direction);
		}

		public void DoAfterMove(Action endCallback)
		{
			if (_moveCoroutine != null)
			{
				this.WaitRoutine(_moveCoroutine, endCallback);
			}
			else
			{
				endCallback?.Invoke();
			}
		}

		private void OnDisable() =>
			_inputHandler.MoveKeyChanged -= OnMoving;

		private void OnMoving(Vector3 direction)
		{
			if (_stateHandler.CurrentState != CubeState.Normal)
			{
				return;
			}

			TryMove(direction);
		}

		private void TryMove(Vector3 direction)
		{
			if (direction == Vector3.zero || (direction.x != 0 && direction.z != 0))
			{
				return;
			}

			if (_moveCoroutine != null || CanMove(direction) is false)
			{
				return;
			}

			_moveCoroutine = StartCoroutine(Move(direction));
		}

		private IEnumerator Move(Vector3 direction)
		{
			StepStarted?.Invoke(direction);

			yield return StartCoroutine(_roll.Move(
				transform,
				direction,
				() => _rollSpeed,
				() => StepEnded?.Invoke()));

			_moveCoroutine = null;
		}

		private bool CanMove(Vector3 direction)
		{
			RaycastHit[] results = new RaycastHit[1];

			var bounds = _cubeCollider.bounds;
			var castDistance = bounds.size.x;

			var hitCount = Physics.BoxCastNonAlloc(
				bounds.center,
				bounds.extents,
				direction,
				results,
				Quaternion.identity,
				castDistance,
				_wallMask);

			return hitCount == 0;
		}
	}
}