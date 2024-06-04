using System;
using System.Collections;
using CubeProject.InputSystem;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Source.Scripts.Game;
using Source.Scripts.Game.Messages;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UniRx;
using UnityEngine;

namespace CubeProject.PlayableCube.Movement
{
	public class CubeMoveService : IDisposable
	{
		private readonly RollMover _roll;
		private readonly PushedHandler _pushedHandler;
		private readonly BoxCollider _cubeCollider;
		private readonly IStateMachine<CubeStateMachine> _cubeStateMachine;
		private readonly IInputService _inputService;
		private readonly LayerMask _wallMask;
		private readonly MonoBehaviour _mono;
		private readonly CompositeDisposable _disposable;

		private Coroutine _moveCoroutine;
		private bool _isMoving;

		public CubeMoveService(
			IStateMachine<CubeStateMachine> stateMachine,
			Transform cubeTransform,
			IInputService inputService,
			MaskHolder maskHolder,
			float rollSpeed,
			BoxCollider cubeCollider,
			MonoBehaviour mono)
		{
			_wallMask = maskHolder.WallMask;
			_cubeStateMachine = stateMachine;
			_inputService = inputService;
			_cubeCollider = cubeCollider;
			_mono = mono;
			_roll = new RollMover(rollSpeed, cubeTransform);
			_pushedHandler = new PushedHandler(_cubeStateMachine);

			_inputService.Moving += _pushedHandler.OnMoving;
			_inputService.Moving += OnMoving;

			_disposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<DoAfterStepMessage>()
				.Subscribe(message => DoAfterStep(message.Action))
				.AddTo(_disposable);

			MessageBroker.Default
				.Receive<PushAfterStepMessage>()
				.Subscribe(message => DoAfterStep(() => Push(message.GetDirection())))
				.AddTo(_disposable);
		}

		public void Dispose()
		{
			_disposable?.Dispose();
			_inputService.Moving -= OnMoving;
			_inputService.Moving -= _pushedHandler.OnMoving;
		}

		private void Push(Vector3 direction)
		{
			if (_cubeStateMachine.CurrentState != typeof(PushState))
			{
				return;
			}

			TryMove(direction);
		}

		private void DoAfterStep(Action endCallback)
		{
			if (_moveCoroutine != null)
			{
				_mono.WaitRoutine(_moveCoroutine, endCallback);
			}
			else
			{
				endCallback?.Invoke();
			}
		}

		private void OnMoving(Vector3 direction)
		{
			if (_cubeStateMachine.CurrentState != typeof(ControlState))
			{
				return;
			}

			TryMove(direction);
		}

		private void TryMove(Vector3 direction)
		{
			if (direction == Vector3.zero || (direction.x != 0 && direction.z != 0))
				return;

			if (_moveCoroutine != null || CanMove(direction) is false)
				return;

			_moveCoroutine = _mono.StartCoroutine(Move(direction));
		}

		private IEnumerator Move(Vector3 direction)
		{
			MessageBroker.Default
				.Publish(new Message<Vector3, CubeMoveService>(MessageId.DirectionChanged, direction));

			MessageBroker.Default
				.Publish(new Message<CubeMoveService>(MessageId.StepStarted));

			_isMoving = true;
			
			yield return _mono.StartCoroutine(_roll.Move(
				direction,
				() => MessageBroker.Default
					.Publish(new Message<CubeMoveService>(MessageId.StepEnded))));

			_isMoving = false;
			_moveCoroutine = null;
		}

		private bool CanMove(Vector3 direction)
		{
			Vector3 halfExtentsOffset = new Vector3(0.01f, 0.01f, 0.01f);
			RaycastHit[] results = new RaycastHit[1];

			var bounds = _cubeCollider.bounds;
			var castDistance = bounds.size.x;

			var hitCount = Physics.BoxCastNonAlloc(
				bounds.center,
				bounds.extents - halfExtentsOffset,
				direction,
				results,
				Quaternion.identity,
				castDistance,
				_wallMask);

			return hitCount == 0;
		}
	}
}