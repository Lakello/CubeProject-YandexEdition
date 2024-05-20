using System;
using System.Linq;
using CubeProject.Game;
using CubeProject.PlayableCube;
using LeadTools.StateMachine;
using Source.Scripts.Game.Messages.ShieldServiceMessage;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UniRx;

namespace Source.Scripts.Game.Level.Shield
{
	public class CubeShieldService : IDisposable
	{
		private readonly Type[] _acceptableStates =
		{
			typeof(ControlState), typeof(PushState), typeof(FallingToGroundState),
		};
		private readonly ChargeHolder _chargeHolder;
		private readonly IStateChangeable<CubeStateMachine> _cubeStateMachine;
		private bool _isAcceptableState;
		private bool _isListenStates;

		public CubeShieldService(Cube cube)
		{
			_chargeHolder = cube.Component.ChargeHolder;
			_cubeStateMachine = cube.Component.StateMachine;

			MessageBroker.Default
				.Publish(new ChangeShieldStateMessage(MessageId.HideShield));

			_chargeHolder.ChargeChanged += OnChargeChanged;
			OnChargeChanged();
		}

		public void Dispose()
		{
			_chargeHolder.ChargeChanged -= OnChargeChanged;
			SetListenStateChanged(false);
		}

		private void OnChargeChanged()
		{
			SetListenStateChanged(_chargeHolder.IsCharged);

			TryChangeShieldState();
		}

		private void OnCubeStateChanged()
		{
			_isAcceptableState =
				_acceptableStates.Any(state => state == _cubeStateMachine.CurrentState);

			TryChangeShieldState();
		}

		private void TryChangeShieldState()
		{
			if (_chargeHolder.IsCharged && _isAcceptableState)
				MessageBroker.Default
					.Publish(new ChangeShieldStateMessage(MessageId.ShowShield));
			else
				MessageBroker.Default
					.Publish(new ChangeShieldStateMessage(MessageId.HideShield));
		}

		private void SetListenStateChanged(bool isListen)
		{
			if (_isListenStates == isListen)
				return;

			_isListenStates = isListen;

			if (_isListenStates)
			{
				_cubeStateMachine.StateChanged += OnCubeStateChanged;
				OnCubeStateChanged();
			}
			else
			{
				_cubeStateMachine.StateChanged -= OnCubeStateChanged;
			}
		}
	}
}