using System;

namespace LeadTools.StateMachine
{
	public interface IStateChangeable<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		public event Action StateChanged;

		public Type CurrentState { get; }

		public void SubscribeTo<TState>(Action<bool> observer)
			where TState : State<TMachine>;

		public void UnSubscribeTo<TState>(Action<bool> observer)
			where TState : State<TMachine>;
	}
}