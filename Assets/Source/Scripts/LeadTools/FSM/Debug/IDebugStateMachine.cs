#if UNITY_EDITOR
namespace LeadTools.StateMachine.Debug
{
	public interface IDebugStateMachine
	{
		public string MachineName { get; }
		public string StateName { get; }
	}
}
#endif