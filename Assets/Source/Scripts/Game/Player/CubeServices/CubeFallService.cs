using System;
using CubeProject.PlayableCube.Movement;
using Source.Scripts.Game;
using Source.Scripts.Game.Messages;
using UniRx;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeFallService : IDisposable
	{
		private readonly GroundChecker _groundChecker;
		private readonly FallHandler _fallHandler;

		private CompositeDisposable _disposable;

		public CubeFallService(
			Cube cube,
			MaskHolder maskHolder)
		{
			_groundChecker = new GroundChecker(maskHolder.GroundMask, cube.transform);
			_fallHandler = new FallHandler(cube, _groundChecker);

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