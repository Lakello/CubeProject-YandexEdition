using LeadTools.FSM.WindowFSM;

namespace LeadTools.FSM.GameFSM.States
{
	public class EndLevelState<TWindowState> : GameState<TWindowState>
		where TWindowState : WindowState
	{
		public EndLevelState(WindowStateMachine windowStateMachine) : base(windowStateMachine)
		{
		}
	}
}