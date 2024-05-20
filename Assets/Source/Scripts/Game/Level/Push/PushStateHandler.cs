using CubeProject.PlayableCube;
using LeadTools.StateMachine;
using Source.Scripts.Game.Messages;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UniRx;

namespace CubeProject.Tips
{
	public class PushStateHandler
	{
		private readonly IStateMachine<CubeStateMachine> _cubeStateMachine;
		private readonly Cube _cube;

		private Pusher _currentPusher;

		public PushStateHandler(Cube cube)
		{
			_cube = cube;
			_cubeStateMachine = _cube.Component.StateMachine;
		}

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