namespace LeadTools.StateMachine
{
	public class GameStateMachine : StateMachine<GameStateMachine>
	{
		public GameStateMachine(WindowStateMachine windowStateMachine) =>
			Window = windowStateMachine;

		public WindowStateMachine Window { get; }
	}
}