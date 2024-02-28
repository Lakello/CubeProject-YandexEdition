using CubeProject.PlayableCube;
using CubeProject.Player;

namespace CubeProject.Tips
{
	public class PusherStateHandler
	{
		private readonly CubeStateHandler _stateHandler;

		private TipKeyPusher _currentPusher;

		public PusherStateHandler(CubeStateHandler stateHandler) =>
			_stateHandler = stateHandler;

		public void Pushing(TipKeyPusher pusher)
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

			_stateHandler.EnterIn(CubeState.Normal);
		}
	}
}