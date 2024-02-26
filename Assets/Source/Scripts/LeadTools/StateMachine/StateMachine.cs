using System;
using System.Collections.Generic;

namespace LeadTools.StateMachine
{
	public abstract class StateMachine<TMachine> : IDisposable, IStateChangeable
		where TMachine : StateMachine<TMachine>
	{
		private Dictionary<Type, State<TMachine>> _states;

		protected StateMachine(Func<Dictionary<Type, State<TMachine>>> getStates) =>
			_states = getStates();

		public event Action<Type> StateChanged;

		public State<TMachine> CurrentState { get; private set; }

		public void Dispose() =>
			CurrentState?.Exit();

		public void EnterIn<TState>()
			where TState : State<TMachine>
		{
			if (_states.ContainsKey(typeof(TState)) == false)
				throw new NullReferenceException(nameof(_states));

			if (!_states.TryGetValue(typeof(TState), out var state))
				return;

			CurrentState?.Exit();
			CurrentState = state;
			CurrentState.Enter();

			StateChanged?.Invoke(CurrentState.GetType());
		}

		protected State<TMachine> TryGetState(Type stateType) =>
			_states.TryGetValue(stateType, out var state) ? state : null;
	}
}