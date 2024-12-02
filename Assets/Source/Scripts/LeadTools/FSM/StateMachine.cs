﻿using System;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using LeadTools.StateMachine.Debug;
#endif

namespace LeadTools.FSM
{
	public abstract class StateMachine<TMachine> :
		IDisposable,
		IStateMachine<TMachine>
#if UNITY_EDITOR
		, IDebugStateMachine
#endif
		where TMachine : StateMachine<TMachine>
	{
		private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		private readonly Dictionary<Type, State<TMachine>> _states = new Dictionary<Type, State<TMachine>>();

		private State<TMachine> _currentState;
		private object[] _stateInstanceParameters;

#if UNITY_EDITOR
		protected StateMachine()
		{
			StateMachineInstancesContainer.Machines.Add(this);
		}
#endif

		public event Action StateChanged;

		public Type CurrentState => _currentState.GetType();

#if UNITY_EDITOR
		public string MachineName => GetType().Name;
		public string StateName => _currentState == null ? string.Empty : CurrentState.Name;
#endif

		public TMachine SetStateInstanceParameters(params object[] stateInstanceParameters)
		{
			_stateInstanceParameters = stateInstanceParameters;

			return (TMachine)this;
		}


		public TMachine AddState<TState>()
			where TState : State<TMachine>
		{
			if (_states.ContainsKey(typeof(TState)) == false)
			{
				var state = (TState)Activator.CreateInstance(typeof(TState), Flags, null, _stateInstanceParameters, null);

				_states.Add(typeof(TState), state);
			}

			return (TMachine)this;
		}

		public void Dispose() =>
			_currentState?.Exit();

		public void EnterIn<TState>()
			where TState : State<TMachine>
		{
			DoWith<TState>((state) =>
			{
				_currentState?.Exit();
				_currentState = state;
				_currentState.Enter();

				StateChanged?.Invoke();
			});
		}

		public void SubscribeTo<TState>(Action<bool> observer)
			where TState : State<TMachine> =>
			DoWith<TState>((state) => state.StateChanged += observer);

		public void UnSubscribeTo<TState>(Action<bool> observer)
			where TState : State<TMachine> =>
			DoWith<TState>((state) => state.StateChanged -= observer);

		protected State<TMachine> TryGetState(Type stateType) =>
			_states.GetValueOrDefault(stateType);

		private void DoWith<TState>(Action<State<TMachine>> action)
			where TState : State<TMachine>
		{
			if (_states.TryGetValue(typeof(TState), out var state))
				action(state);
			else
				throw new ArgumentException();
		}
	}
}