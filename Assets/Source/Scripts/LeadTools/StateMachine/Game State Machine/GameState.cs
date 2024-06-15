namespace LeadTools.StateMachine
{
	public abstract class GameState<TWindowState> : State<GameStateMachine>
		where TWindowState : WindowState
	{
		private readonly WindowStateMachine _windowStateMachine;

		protected GameState(WindowStateMachine windowStateMachine) =>
			_windowStateMachine = windowStateMachine;

		public override void Enter()
		{
			base.Enter();

			_windowStateMachine.EnterIn<TWindowState>();
		}
	}
}