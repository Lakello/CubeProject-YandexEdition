using System;
using System.Collections.Generic;

namespace LeadTools.StateMachine
{
	public class GameStateMachine : StateMachine<GameStateMachine>
	{
		public GameStateMachine(WindowStateMachine windowStateMachine, Func<Dictionary<Type, State<GameStateMachine>>> getStates)
			: base(getStates) =>
			Window = windowStateMachine;

		public WindowStateMachine Window { get; }
	}
}