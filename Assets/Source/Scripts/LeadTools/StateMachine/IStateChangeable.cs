using System;

namespace LeadTools.StateMachine
{
	public interface IStateChangeable
	{
		public event Action<Type> StateChanged;
	}
}