using System;

namespace LeadTools.StateMachine.States
{
	public class MenuState<TWindowState> : GameState
		where TWindowState : WindowState
	{
		public MenuState(Action enterInWindowState) : base(enterInWindowState)
		{
		}
	}
}