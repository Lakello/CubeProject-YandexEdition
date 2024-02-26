using LeadTools.StateMachine;

namespace LeadTools.TypedScenes
{
	public interface ISceneLoadHandlerOnState<TMachine>
		where TMachine : StateMachine<TMachine>
	{
		public void OnSceneLoaded<TState>(TMachine machine)
			where TState : State<TMachine>;
	}
}