using Cinemachine;
using LeadTools.Extensions;
using UnityEngine;

namespace Source.Scripts.Game.Level.Camera
{
	public class TargetCameraHolder
	{
		private readonly float _delayStopCameraFollow = 0.3f;
		private readonly MonoBehaviour _mono;
		private readonly CinemachineVirtualCamera _virtualCamera;

		private Transform _lookAtPoint;
		private Transform _followPoint;

		public TargetCameraHolder(MonoBehaviour mono, CinemachineVirtualCamera virtualCamera)
		{
			_mono = mono;
			_virtualCamera = virtualCamera;
		}

		public void Init(Transform lookAtPoint, Transform followPoint)
		{
			_lookAtPoint = lookAtPoint;
			_followPoint = followPoint;
		}

		public void ResetTarget() =>
			_mono.WaitTime(
				_delayStopCameraFollow,
				() =>
				{
					_virtualCamera.LookAt = null;
					_virtualCamera.Follow = null;
				});

		public void SetTarget()
		{
			_virtualCamera.LookAt = _lookAtPoint;
			_virtualCamera.Follow = _followPoint;
		}
	}
}