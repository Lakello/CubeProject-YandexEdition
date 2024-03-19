using System;
using System.Collections.Generic;
using LeadTools.StateMachine;

namespace Source.Scripts.Game.tateMachine
{
	public class CubeStateMachine : StateMachine<CubeStateMachine>
	{
		public CubeStateMachine(Func<Dictionary<Type, State<CubeStateMachine>>> getStates) : base(getStates)
		{
		}
	}
}