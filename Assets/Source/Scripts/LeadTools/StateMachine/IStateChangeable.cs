using System;

namespace LeadTools.StateMachine
{
	public interface IStateChangeable<TMachine> 
		where TMachine : StateMachine<TMachine>
	{
		public void SubscribeTo<TState>(Action observer)
			where TState : State<TMachine>;

		public void UnSubscribeTo<TState>(Action observer)
			where TState : State<TMachine>;
	}
}