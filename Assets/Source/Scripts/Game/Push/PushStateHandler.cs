using CubeProject.PlayableCube;

namespace CubeProject.Tips
{
	public class PushStateHandler
	{
		private readonly CubeStateHandler _stateHandler;
		private readonly Cube _cube;

		private Pusher _currentPusher;

		public PushStateHandler(Cube cube)
		{
			_cube = cube;
			_stateHandler = _cube.ServiceHolder.StateHandler;
		}

		public void Pushing(Pusher pusher)
		{
			if (_currentPusher is not null)
			{
				_currentPusher.Pushed -= OnPushed;
			}

			_currentPusher = pusher;
			_currentPusher.Pushed += OnPushed;

			_stateHandler.EnterIn(CubeState.Pushing);
		}

		private void OnPushed()
		{
			_currentPusher.Pushed -= OnPushed;
			_currentPusher = null;

			if (_cube.ServiceHolder.FallService.TryFall() is false)
			{
				_stateHandler.EnterIn(CubeState.Normal);
			}
		}
	}
}