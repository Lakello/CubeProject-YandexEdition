namespace LeadTools.StateMachine
{
	public interface IStateMachine<TMachine> : IStateChangeable<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		public void EnterIn<TState>()
			where TState : State<TMachine>;
	}
}