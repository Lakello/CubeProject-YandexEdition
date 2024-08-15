using System;
using System.Linq;
using CubeProject.Game.PlayerStateMachine;
using CubeProject.Game.PlayerStateMachine.States;
using Game.Player.Shield.States;
using LeadTools.StateMachine;

namespace Game.Player.Shield
{
	public class CubeShieldService : IDisposable
	{
		private readonly Type[] _acceptableStates =
		{
			typeof(ControlState), typeof(PushState), typeof(FallingToGroundState),
		};
		private readonly ChargeHolder _chargeHolder;
		private readonly IStateChangeable<CubeStateMachine> _cubeStateMachine;
		private readonly IStateMachine<ShieldStateMachine> _shieldStateMachine;

		private bool _isAcceptableState;
		private bool _isListenStates;

		public CubeShieldService(CubeEntity cubeEntity, IStateMachine<ShieldStateMachine> shieldStateMachine)
		{
			_chargeHolder = cubeEntity.Component.ChargeHolder;
			_cubeStateMachine = cubeEntity.Component.StateMachine;

			_shieldStateMachine = shieldStateMachine;

			_shieldStateMachine.EnterIn<StopState>();

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
			{
				if (_shieldStateMachine.CurrentState != typeof(PlayState))
					_shieldStateMachine.EnterIn<PlayState>();
			}
			else
			{
				if (_shieldStateMachine.CurrentState != typeof(StopState))
					_shieldStateMachine.EnterIn<StopState>();
			}
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