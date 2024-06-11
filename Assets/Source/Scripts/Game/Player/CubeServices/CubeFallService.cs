using System;
using CubeProject.Game.Player;
using CubeProject.Game.Messages;
using CubeProject.Game.Player.Movement;
using UniRx;

namespace CubeProject.Game.Player
{
	public class CubeFallService : IDisposable
	{
		private readonly GroundChecker _groundChecker;
		private readonly FallHandler _fallHandler;

		private CompositeDisposable _disposable;

		public CubeFallService(
			CubeEntity cubeEntity,
			MaskHolder maskHolder)
		{
			_groundChecker = new GroundChecker(maskHolder.GroundMask, cubeEntity.transform);
			_fallHandler = new FallHandler(cubeEntity, _groundChecker);

			_disposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<Message<CubeMoveService>>()
				.Where(message => message.Id == MessageId.StepEnded)
				.Subscribe(_ => OnStepEnded())
				.AddTo(_disposable);

			MessageBroker.Default
				.Receive<CheckGroundMessage>()
				.Subscribe(message => message.Callback.Invoke(TryFall()))
				.AddTo(_disposable);

			TryFall();
		}

		public void Dispose()
		{
			_disposable?.Dispose();
			_fallHandler.Dispose();
		}

		private bool TryFall()
		{
			if (_groundChecker.IsGrounded())
			{
				return false;
			}
			else
			{
				_fallHandler.Play();

				return true;
			}
		}

		private void OnStepEnded() =>
			TryFall();
	}
}