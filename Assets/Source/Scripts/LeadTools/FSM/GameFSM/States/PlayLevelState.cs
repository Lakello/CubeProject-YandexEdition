using LeadTools.FSM.WindowFSM;

namespace LeadTools.FSM.GameFSM.States
{
	public class PlayLevelState<TWindowState> : GameState<TWindowState>
		where TWindowState : WindowState
	{
		public PlayLevelState(WindowStateMachine windowStateMachine) : base(windowStateMachine)
		{
		}
	}
}