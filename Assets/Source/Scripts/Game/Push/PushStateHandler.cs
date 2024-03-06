using CubeProject.PlayableCube;

namespace CubeProject.Tips
{
	public class PushStateHandler
	{
		private readonly CubeStateService _stateService;
		private readonly Cube _cube;

		private Pusher _currentPusher;

		public PushStateHandler(Cube cube)
		{
			_cube = cube;
			_stateService = _cube.ServiceHolder.StateService;
		}

		public void Pushing(Pusher pusher)
		{
			if (_currentPusher is not null)
			{
				_currentPusher.Pushed -= OnPushed;
			}

			_currentPusher = pusher;
			_currentPusher.Pushed += OnPushed;

			_stateService.EnterIn(CubeState.Pushing);
		}

		private void OnPushed()
		{
			_currentPusher.Pushed -= OnPushed;
			_currentPusher = null;

			if (_cube.ServiceHolder.FallService.TryFall() is false)
			{
				_stateService.EnterIn(CubeState.Normal);
			}
		}
	}
}