using System;
using Cinemachine;
using CubeProject.PlayableCube;
using Source.Scripts.Game.Messages;
using Source.Scripts.Game.Messages.Camera;
using UniRx;
using UnityEngine;

namespace Source.Scripts.Game.Level.Camera
{
	public class TargetCameraHolder : IDisposable
	{
		private readonly float _delayStopCameraFollow = 0.3f;
		private readonly CinemachineVirtualCamera _virtualCamera;
		private readonly CompositeDisposable _disposable;

		private Transform _lookAtPoint;
		private Transform _followPoint;

		public TargetCameraHolder(CinemachineVirtualCamera virtualCamera, Transform lookAtPoint, Transform followPoint)
		{
			_virtualCamera = virtualCamera;
			_disposable = new CompositeDisposable();

			MessageBroker.Default
				.Receive<Message<CubeFallService>>()
				.Where(message => message.Id == MessageId.FallingIntoAbyss)
				.Subscribe(_ => ResetTarget())
				.AddTo(_disposable);

			MessageBroker.Default
				.Receive<SetTargetCameraMessage>()
				.Subscribe(_ => SetTarget())
				.AddTo(_disposable);
			
			_lookAtPoint = lookAtPoint;
			_followPoint = followPoint;
			
			SetTarget();
		}

		public void Dispose() =>
			_disposable?.Dispose();

		private void SetTarget()
		{
			_virtualCamera.LookAt = _lookAtPoint;
			_virtualCamera.Follow = _followPoint;
		}

		private void ResetTarget() =>
			Observable.Timer(TimeSpan.FromSeconds(_delayStopCameraFollow))
				.Subscribe(_ =>
				{
					_virtualCamera.LookAt = null;
					_virtualCamera.Follow = null;
				})
				.AddTo(_disposable);
	}
}