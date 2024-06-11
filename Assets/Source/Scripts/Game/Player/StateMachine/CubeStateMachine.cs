using System;
using System.Collections.Generic;
using LeadTools.StateMachine;

namespace CubeProject.Game.PlayerStateMachine
{
	public class CubeStateMachine : StateMachine<CubeStateMachine>
	{
		public CubeStateMachine(Func<Dictionary<Type, State<CubeStateMachine>>> getStates) : base(getStates)
		{
		}
	}
}