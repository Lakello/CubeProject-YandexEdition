using System;

namespace LeadTools.StateMachine
{
	public abstract class State<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		public event Action Entered;

		public virtual void Enter() =>
			Entered?.Invoke();

		public abstract void Exit();
	}
}