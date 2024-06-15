using CubeProject.Game.Messages;
using CubeProject.Game.Player;
using CubeProject.Game.PlayerStateMachine;
using CubeProject.Game.PlayerStateMachine.States;
using LeadTools.StateMachine;
using UniRx;

namespace CubeProject.Tips
{
	public class PushStateHandler
	{
		private readonly IStateMachine<CubeStateMachine> _cubeStateMachine;

		private Pusher _currentPusher;

		public PushStateHandler(CubeComponent cubeComponent) =>
			_cubeStateMachine = cubeComponent.StateMachine;

		public void Pushing(Pusher pusher)
		{
			if (_currentPusher is not null)
				_currentPusher.Pushed -= OnPushed;

			_currentPusher = pusher;
			_currentPusher.Pushed += OnPushed;

			_cubeStateMachine.EnterIn<PushState>();
		}

		private void OnPushed()
		{
			_currentPusher.Pushed -= OnPushed;
			_currentPusher = null;

			MessageBroker.Default
				.Publish(new CheckGroundMessage(isGrounded =>
				{
					if (isGrounded is false)
						_cubeStateMachine.EnterIn<ControlState>();
				}));
		}
	}
}