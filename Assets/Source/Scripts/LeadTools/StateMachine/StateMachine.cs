using System;
using System.Collections.Generic;

namespace LeadTools.StateMachine
{
	public abstract class StateMachine<TMachine> : IDisposable, IStateChangeable<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		private readonly Dictionary<Type, State<TMachine>> _states;

		protected StateMachine(Func<Dictionary<Type, State<TMachine>>> getStates) =>
			_states = getStates();

		public State<TMachine> CurrentState { get; private set; }

		public void Dispose() =>
			CurrentState?.Exit();

		public void EnterIn<TState>()
			where TState : State<TMachine>
		{
			DoWith<TState>((state) =>
			{
				CurrentState?.Exit();
				CurrentState = state;
				CurrentState.Enter();
			});
		}

		public void SubscribeTo<TState>(Action observer)
			where TState : State<TMachine> =>
			DoWith<TState>((state) => state.Entered += observer);

		public void UnSubscribeTo<TState>(Action observer)
			where TState : State<TMachine> =>
			DoWith<TState>((state) => state.Entered -= observer);

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