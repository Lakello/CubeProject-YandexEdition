using System;
using System.Collections.Generic;
using System.Linq;
using CubeProject.Game;
using CubeProject.PlayableCube;
using LeadTools.StateMachine;
using Source.Scripts.Game.Level.Shield.States;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace Source.Scripts.Game.Level.Shield
{
	[Serializable]
	public class CubeShieldService : IDisposable
	{
		private readonly Type[] _acceptableStates =
		{
			typeof(ControlState), typeof(PushState),
		};
		private readonly ChargeHolder _chargeHolder;
		private readonly IStateChangeable<CubeStateMachine> _stateChangeable;
		private readonly IStateMachine<ShieldStateMachine> _stateMachine;

		[SerializeField] private bool _isAcceptableState;
		[SerializeField] private bool _isListenStates;

		public CubeShieldService(Cube cube)
		{
			_chargeHolder = cube.Component.ChargeHolder;
			_stateChangeable = cube.Component.StateMachine;
			
			_stateMachine = new ShieldStateMachine(
				() => new Dictionary<Type, State<ShieldStateMachine>>
				{
					[typeof(PlayState)] = new PlayState(),
					[typeof(StopState)] = new StopState(),
				});
			
			_stateMachine.EnterIn<StopState>();
			
			_chargeHolder.ChargeChanged += OnChargeChanged; 
			OnChargeChanged();
		}

		public IStateChangeable<ShieldStateMachine> StateMachine => _stateMachine;

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
			_isAcceptableState = 
				_acceptableStates.Any(state => state == _stateChangeable.CurrentState);

			TryChangeShieldState();
		}

		private void TryChangeShieldState()
		{
			if (_chargeHolder.IsCharged && _isAcceptableState)
			{
				if (_stateMachine.CurrentState != typeof(PlayState))
					_stateMachine.EnterIn<PlayState>();
			}
			else
			{
				if (_stateMachine.CurrentState != typeof(StopState))
					_stateMachine.EnterIn<StopState>();
			}
		}

		private void SetListenStateChanged(bool isListen)
		{
			if (_isListenStates == isListen)
				return;

			_isListenStates = isListen;

			if (_isListenStates)
			{
				_stateChangeable.StateChanged += OnStateChanged;
				OnStateChanged();
			}
			else
			{
				_stateChangeable.StateChanged -= OnStateChanged;
			}
		}
	}
}