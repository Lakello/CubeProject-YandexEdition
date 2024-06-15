namespace LeadTools.StateMachine.States
{
	public class EndLevelState<TWindowState> : GameState<TWindowState>
		where TWindowState : WindowState
	{
		public EndLevelState(WindowStateMachine windowStateMachine) : base(windowStateMachine)
		{
		}
	}
}