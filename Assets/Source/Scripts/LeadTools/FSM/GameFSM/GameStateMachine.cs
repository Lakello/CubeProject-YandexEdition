using LeadTools.FSM.WindowFSM;

namespace LeadTools.FSM.GameFSM
{
	public class GameStateMachine : StateMachine<GameStateMachine>
	{
		public GameStateMachine(WindowStateMachine windowStateMachine) =>
			Window = windowStateMachine;

		public WindowStateMachine Window { get; }
	}
}