using System;
using CubeProject.Game.Player.CubeService.Fall;
using CubeProject.Game.Player.CubeService.Messages;
using LeadTools.Common;
using UniRx;

namespace CubeProject.Game.Player.CubeService
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
				.Receive<M_StepEnded>()
				.Subscribe(_ => OnStepEnded())
				.AddTo(_disposable);

			MessageBroker.Default
				.Receive<M_CheckGround>()
				.Subscribe(message => message.Data.Invoke(TryFall()))
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

			_fallHandler.Play();

			return true;
		}

		private void OnStepEnded() =>
			TryFall();
	}
}