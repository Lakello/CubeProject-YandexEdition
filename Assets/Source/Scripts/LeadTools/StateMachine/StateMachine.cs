using System;
using System.Collections.Generic;

namespace LeadTools.StateMachine
{
	public abstract class StateMachine<TMachine> : IDisposable, IStateMachine<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		private readonly Dictionary<Type, State<TMachine>> _states;

		private State<TMachine> _currentState;

		protected StateMachine(Func<Dictionary<Type, State<TMachine>>> getStates) =>
			_states = getStates();

		public Type CurrentState => _currentState.GetType();

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
			});
		}

		public void SubscribeTo<TState>(Action<bool> observer)
			where TState : State<TMachine> =>
			DoWith<TState>((state) => state.StateChanged += observer);

		public void UnSubscribeTo<TState>(Action<bool> observer)
			where TState : State<TMachine> =>
			DoWith<TState>((state) => state.StateChanged -= observer);

		protected State<TMachine> TryGetState(Type stateType) =>
			_states.TryGetValue(stateType, out var state) ? state : null;

		private void DoWith<TState>(Action<State<TMachine>> action)
			where TState : State<TMachine>
		{
			if (_states.TryGetValue(typeof(TState), out var state))
			{
				action(state);
			}
			else
			{
				throw new ArgumentException();
			}
		}
	}
}