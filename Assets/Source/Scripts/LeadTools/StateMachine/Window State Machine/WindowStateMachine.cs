namespace LeadTools.StateMachine
{
	public class WindowStateMachine : StateMachine<WindowStateMachine>
	{
		public TState TryGetState<TState>(Window window)
			where TState : State<WindowStateMachine> =>
			(TState)TryGetState(window.WindowType);
	}
}