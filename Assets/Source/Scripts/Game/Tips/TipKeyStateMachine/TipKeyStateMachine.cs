using System;
using System.Collections.Generic;
using LeadTools.StateMachine;

namespace CubeProject.Tips.FSM
{
	public class TipKeyStateMachine : StateMachine<TipKeyStateMachine>
	{
		public TipKeyStateMachine(Func<Dictionary<Type, State<TipKeyStateMachine>>> getStates) : base(getStates)
		{
		}
	}
}