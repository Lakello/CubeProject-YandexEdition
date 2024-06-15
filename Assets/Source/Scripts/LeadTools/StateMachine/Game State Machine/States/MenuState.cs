namespace LeadTools.StateMachine.States
{
	public class MenuState<TWindowState> : GameState<TWindowState>
		where TWindowState : WindowState
	{
		public MenuState(WindowStateMachine windowStateMachine) : base(windowStateMachine)
		{
		}
	}
}