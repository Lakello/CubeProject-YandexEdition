using System;
using System.Threading;
using CubeProject.Game.InputSystem;
using CubeProject.Game.Player.CubeService.Messages;
using CubeProject.Game.Player.CubeService.Movement;
using CubeProject.Game.Player.FSM;
using CubeProject.Game.Player.FSM.States;
using Cysharp.Threading.Tasks;
using LeadTools.FSM;
using UniRx;
using UnityEngine;

namespace CubeProject.Game.Player.CubeService
{
	public class CubeMoveService : IDisposable
	{
		private readonly RollMover _roll;
		private readonly BoxCollider _cubeCollider;
		private readonly IStateMachine<CubeStateMachine> _cubeStateMachine;
		private readonly IInputService _inputService;
		private readonly LayerMask _wallMask;
		private readonly CompositeDisposable _messageDisposable;
		private readonly M_MoveDirectionChanged _directionChangedMessage = new M_MoveDirectionChanged();
		private readonly M_StepStarted _stepStartedMessage = new M_StepStarted();
		private readonly M_StepEnded _stepEndedMessage = new M_StepEnded();

		private Action _endMoveAction;
		private CancellationTokenSource _moveCancellationToken;

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
				.Receive<M_DoAfterStep>()
				.Subscribe(message => DoAfterStep(message.Action))
				.AddTo(_messageDisposable);

			MessageBroker.Default
				.Receive<M_PushAfterStep>()
				.Subscribe(message => DoAfterStep(() => Push(message.GetDirection())))
				.AddTo(_messageDisposable);
		}

		public void Dispose()
		{
			_moveCancellationToken?.Dispose();
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
			if (_moveCancellationToken != null)
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

			if (_moveCancellationToken != null || CanMove(direction) is false)
				return;

			Move(direction);
		}

		private async void Move(Vector3 direction)
		{
			_moveCancellationToken = new CancellationTokenSource();

			MessageBroker.Default
				.Publish(_directionChangedMessage.SetData(direction));

			MessageBroker.Default
				.Publish(_stepStartedMessage);

			await UniTask.Create(token => _roll.Move(direction, token), _moveCancellationToken.Token);

			OnEndMove();
		}

		private void OnEndMove()
		{
			MessageBroker.Default
				.Publish(_stepEndedMessage);

			_moveCancellationToken?.Dispose();
			_moveCancellationToken = null;

			_endMoveAction?.Invoke();
			_endMoveAction = null;
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