using System;
using System.Collections.Generic;

namespace LeadTools.StateMachine
{
	public class WindowStateMachine : StateMachine<WindowStateMachine>
	{
		public WindowStateMachine(Func<Dictionary<Type, State<WindowStateMachine>>> getStates) : base(getStates)
		{
		}

		public TState TryGetState<TState>(Window window)
			where TState : State<WindowStateMachine> =>
			(TState)TryGetState(window.WindowType);
	}
}