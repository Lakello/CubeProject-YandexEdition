using System;

namespace LeadTools.StateMachine.States
{
	public class EndLevelState : GameState
	{
		public EndLevelState(Action enterInWindowState) : base(enterInWindowState)
		{
		}
	}
}