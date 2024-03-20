using System;

namespace LeadTools.StateMachine
{
	public abstract class GameState : State<GameStateMachine>
	{
		private readonly Action _enterInWindowState;

		protected GameState(Action enterInWindowState) =>
			_enterInWindowState = enterInWindowState;

		public override void Enter()
		{
			base.Enter();
			_enterInWindowState();
		}
	}
}