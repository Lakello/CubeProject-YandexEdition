using System;
using System.Collections.Generic;
using LeadTools.StateMachine;

namespace Source.Scripts.Game.Level.Shield
{
	public class ShieldStateMachine : StateMachine<ShieldStateMachine>
	{
		public ShieldStateMachine(Func<Dictionary<Type, State<ShieldStateMachine>>> getStates) : base(getStates)
		{
		}
	}
}