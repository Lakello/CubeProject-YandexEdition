using System;
using System.Collections;
using CubeProject.Game.Messages;
using CubeProject.Game.PlayerStateMachine;
using CubeProject.Game.PlayerStateMachine.States;
using LeadTools.StateMachine;
using UniRx;
using UnityEngine;

namespace CubeProject.Game.Player
{
	public class FallHandler : IDisposable
	{
		private readonly float _speedFall = 3;
		private readonly float _speedFallMultiplier = 1.1f;
		private readonly Vector3 _offset = new Vector3(0, 0.5f, 0);

		private readonly IStateMachine<CubeStateMachine> _cubeStateMachine;
		private readonly GroundChecker _groundChecker;
		private readonly CubeEntity _cubeEntity;
		private readonly BecameVisibleBehaviour _became;
		private readonly CompositeDisposable _disposable;

		public FallHandler(CubeEntity cubeEntity, GroundChecker groundChecker)
		{
			_disposable = new CompositeDisposable();

			_cubeEntity = cubeEntity;
			_became = _cubeEntity.Component.BecameVisibleBehaviour;
			_cubeStateMachine = _cubeEntity.Component.StateMachine;
			_groundChecker = groundChecker;
		}

		private Vector3 Position
		{
			get => _cubeEntity.transform.position - _offset;
			set => _cubeEntity.transform.position = _offset + value;
		}

		public void Dispose() =>
			_disposable?.Dispose();

		public void Play()
		{
			Func<float, Vector3> calculatePosition;
			Func<bool> whileCondition;
			Action endCallback;

			if (CanFallToGround(out var groundPositionY))
			{
				_cubeStateMachine.EnterIn<FallingToGroundState>();

				(calculatePosition, whileCondition, endCallback) = GetFallIntoGroundData(groundPositionY);
			}
			else
			{
				_cubeStateMachine.EnterIn<FallingToAbyssState>();

				MessageBroker.Default
					.Publish(new Message<CubeFallService>(MessageId.FallingIntoAbyss));

				(calculatePosition, whileCondition, endCallback) = GetFallIntoAbyssData();
			}

			Observable.FromCoroutine(() => Fall(calculatePosition, whileCondition))
				.Subscribe(_ => endCallback?.Invoke())
				.AddTo(_disposable);
		}

		private bool CanFallToGround(out float groundPositionY)
		{
			return _groundChecker.IsGrounded(_cubeEntity.transform.position, Mathf.Infinity, out groundPositionY);
		}

		private IEnumerator Fall(Func<float, Vector3> calculatePosition, Func<bool> whileCondition)
		{
			var wait = new WaitForFixedUpdate();
			var speed = _speedFall;

			while (whileCondition())
			{
				var delta = (speed *= _speedFallMultiplier) * Time.fixedDeltaTime;

				Position = calculatePosition(delta);

				yield return wait;
			}
		}

		private (Func<float, Vector3> calculatePosition, Func<bool> whileCondition, Action endCallback)
			GetFallIntoGroundData(float groundPositionY) =>
			(delta =>
			{
				var position = Position;

				if (position.y - delta < groundPositionY)
					position.y = groundPositionY;
				else
					position.y -= delta;

				return position;
			},
			() => _groundChecker.IsGrounded() is false && groundPositionY < Position.y,
			() => _cubeStateMachine.EnterIn<ControlState>());

		private (Func<float, Vector3> calculatePosition, Func<bool> whileCondition, Action endCallback)
			GetFallIntoAbyssData() =>
			(delta =>
			{
				var position = Position;
				position.y -= delta;

				return position;
			},
			() => _became.IsVisible,
			_cubeEntity.Kill);
	}
}