using LeadTools.FSM.WindowFSM;

namespace LeadTools.FSM.GameFSM.States
{
	public class MenuState<TWindowState> : GameState<TWindowState>
		where TWindowState : WindowState
	{
		public MenuState(WindowStateMachine windowStateMachine) : base(windowStateMachine)
		{
		}
	}
}