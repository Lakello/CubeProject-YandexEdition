namespace LeadTools.StateMachine.States
{
	public class PlayLevelState<TWindowState> : GameState<TWindowState>
		where TWindowState : WindowState
	{
		public PlayLevelState(WindowStateMachine windowStateMachine) : base(windowStateMachine)
		{
		}
	}
}