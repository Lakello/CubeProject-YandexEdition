using Cinemachine;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using UnityEngine;

namespace Source.Scripts.Game.Level.Camera
{
	public class TargetCameraHolder
	{
		private readonly float _delayStopCameraFollow = 0.3f;
		private readonly MonoBehaviour _mono;
		private readonly CinemachineVirtualCamera _virtualCamera;
		private readonly Transform _lookAtPoint;
		private readonly Transform _followPoint;
		
		public TargetCameraHolder(MonoBehaviour mono, CinemachineVirtualCamera virtualCamera, Transform lookAtPoint, Transform followPoint)
		{
			_mono = mono;
			_virtualCamera = virtualCamera;
			_lookAtPoint = lookAtPoint;
			_followPoint = followPoint;
			
			virtualCamera.Follow = _followPoint;
			virtualCamera.LookAt = _lookAtPoint;
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