using System;
using System.Collections;
using CubeProject.InputSystem;
using CubeProject.PlayableCube;
using CubeProject.PlayableCube.Movement;
using LeadTools.Extensions;
using Reflex.Attributes;
using Source.Scripts.Game;
using UnityEngine;

namespace CubeProject.PlayableCube.Movement
{
	public class CubeMoveService : MonoBehaviour
	{
		[SerializeField] private float _rollSpeed;
		[SerializeField] private AudioSource _audioSourceMove;

		private RollMover _roll;
		private BoxCollider _cubeCollider;
		private CubeStateHandler _stateHandler;
		private Coroutine _moveCoroutine;
		private bool _isBrake;
		private IInputService _inputService;
		private LayerMask _wallMask;

		public event Action StepEnded;

		public event Action<Vector3> StepStarted;

		public Vector3 CurrentDirection { get; private set; }

		[Inject]
		private void Inject(Cube cube, IInputService inputService, MaskHolder maskHolder)
		{
			_wallMask = maskHolder.WallMask;
			
			_stateHandler = cube.ServiceHolder.StateHandler;
			_cubeCollider = cube.ServiceHolder.SelfCollider;

			_inputService = inputService;

			_inputService.Moving += OnMoving;
		}

		private void Awake() =>
			_roll = new RollMover(_audioSourceMove);

		private void OnDisable() =>
			_inputService.Moving -= OnMoving;

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
			CurrentDirection = direction;
			
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
				bounds.extents - new Vector3(0.01f, 0.01f, 0.01f),
				direction,
				results,
				Quaternion.identity,
				castDistance,
				_wallMask);

			return hitCount == 0;
		}
	}
}