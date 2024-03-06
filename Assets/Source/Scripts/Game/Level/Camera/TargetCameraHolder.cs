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
		private readonly Cube _cube;
		
		public TargetCameraHolder(MonoBehaviour mono, CinemachineVirtualCamera virtualCamera, Cube cube)
		{
			_mono = mono;
			_virtualCamera = virtualCamera;
			_cube = cube;
		}
		
		public void ResetLookAt() =>
			_mono.WaitTime(_delayStopCameraFollow, () => _virtualCamera.LookAt = null);

		public void SetLookAt() =>
			_virtualCamera.LookAt = _cube.transform;
	}
}