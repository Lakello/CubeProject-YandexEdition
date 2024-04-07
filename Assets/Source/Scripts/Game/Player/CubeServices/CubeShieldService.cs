using System;
using System.Linq;
using CubeProject.Game;
using CubeProject.PlayableCube;
using LeadTools.StateMachine;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;

namespace Source.Scripts.Game.Level.Shield
{
	public class CubeShieldService : IDisposable
	{
		private readonly Type[] _acceptableStates =
		{
			typeof(ControlState), typeof(PushState),
		};
		private readonly ChargeHolder _chargeHolder;
		private readonly IStateChangeable<CubeStateMachine> _stateChangeable;
		private readonly ShieldView _shieldView;

		private bool _isAcceptableState;
		private bool _isListenStates;

		public CubeShieldService(Cube cube, ShieldView shieldView)
		{
			_chargeHolder = cube.Component.ChargeHolder;
			_stateChangeable = cube.Component.StateMachine;
			_shieldView = shieldView;

			_chargeHolder.ChargeChanged += OnChargeChanged;
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

		private void OnStateChanged()
		{
			if (_acceptableStates.Any(state => state == _stateChangeable.CurrentState))
			{
				ChangeIsAcceptableState(true);

				return;
			}

			ChangeIsAcceptableState(false);

			return;

			void ChangeIsAcceptableState(bool value)
			{
				_isAcceptableState = value;
				TryChangeShieldState();
			}
		}

		private void TryChangeShieldState()
		{
			if (_chargeHolder.IsCharged && _isAcceptableState)
				_shieldView.Play();
			else
				_shieldView.Stop();
		}

		private void SetListenStateChanged(bool isListen)
		{
			if (_isListenStates == isListen)
				return;

			_isListenStates = isListen;

			if (_isListenStates)
				_stateChangeable.StateChanged += OnStateChanged;
			else
				_stateChangeable.StateChanged -= OnStateChanged;
		}
	}
}