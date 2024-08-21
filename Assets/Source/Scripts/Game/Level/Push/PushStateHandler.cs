using CubeProject.Game.Player.CubeService;
using CubeProject.Game.Player.CubeService.Messages;
using CubeProject.Game.Player.FSM;
using CubeProject.Game.Player.FSM.States;
using LeadTools.FSM;
using UniRx;

namespace CubeProject.Game.Level.Push
{
	public class PushStateHandler
	{
		private readonly IStateMachine<CubeStateMachine> _cubeStateMachine;
		private readonly M_CheckGround _checkGroundMessage = new M_CheckGround();

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
				.Publish(_checkGroundMessage.SetData(isGrounded =>
				{
					if (isGrounded is false)
						_cubeStateMachine.EnterIn<ControlState>();
				}));
		}
	}
}