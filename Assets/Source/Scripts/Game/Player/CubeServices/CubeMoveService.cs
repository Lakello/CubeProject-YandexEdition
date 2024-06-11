using System;
using CubeProject.Game.Messages;
using CubeProject.Game.PlayerStateMachine;
using CubeProject.Game.PlayerStateMachine.States;
using CubeProject.InputSystem;
using LeadTools.StateMachine;
using UniRx;
using UnityEngine;

namespace CubeProject.Game.Player.Movement
{
	public class CubeMoveService : IDisposable
	{
		private readonly RollMover _roll;
		private readonly BoxCollider _cubeCollider;
		private readonly IStateMachine<CubeStateMachine> _cubeStateMachine;
		private readonly IInputService _inputService;
		private readonly LayerMask _wallMask;
		private readonly CompositeDisposable _messageDisposable;

		private Action _endMoveAction;
		private CompositeDisposable _moveDisposable;

		public CubeMoveService(
			IStateMachine<CubeStateMachine> stateMachine,
			Transform cubeTransform,
			IInputService inputService,
			MaskHolder maskHolder,
			float rollSpeed,
			BoxCollider cubeCollider)
		{
			_wallMask = maskHolder.WallMask;
			_cubeStateMachine = stateMachine;
			_inputService = inputService;
			_cubeCollider = cubeCollider;
			_roll = new RollMover(rollSpeed, cubeTransform);

			_inputService.Moving += OnMoving;

			_messageDisposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<DoAfterStepMessage>()
				.Subscribe(message => DoAfterStep(message.Action))
				.AddTo(_messageDisposable);

			MessageBroker.Default
				.Receive<PushAfterStepMessage>()
				.Subscribe(message => DoAfterStep(() => Push(message.GetDirection())))
				.AddTo(_messageDisposable);
		}

		public void Dispose()
		{
			_moveDisposable?.Dispose();
			_messageDisposable?.Dispose();
			_inputService.Moving -= OnMoving;
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
			if (_moveDisposable != null)
			{
				_endMoveAction = endCallback;
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

			if (_moveDisposable != null || CanMove(direction) is false)
				return;

			Move(direction);
		}

		private void Move(Vector3 direction)
		{
			_moveDisposable = new CompositeDisposable();

			MessageBroker.Default
				.Publish(new Message<Vector3, CubeMoveService>(MessageId.DirectionChanged, direction));

			MessageBroker.Default
				.Publish(new Message<CubeMoveService>(MessageId.StepStarted));

			Observable.FromCoroutine(
					() => _roll.Move(
						direction))
				.Subscribe(_ => OnEndMove())
				.AddTo(_moveDisposable);
		}

		private void OnEndMove()
		{
			MessageBroker.Default
				.Publish(new Message<CubeMoveService>(MessageId.StepEnded));

			_endMoveAction?.Invoke();
			_endMoveAction = null;

			_moveDisposable.Dispose();
			_moveDisposable = null;
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