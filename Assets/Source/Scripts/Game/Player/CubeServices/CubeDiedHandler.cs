using Cinemachine;
using CubeProject.Game;
using CubeProject.PlayableCube;
using Reflex.Attributes;
using Source.Scripts.Game.Level.Camera;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeDiedHandler : MonoBehaviour
	{
		[SerializeField] private Transform _cameraFollow;
		[SerializeField] private Vector3 _offset = new Vector3(0, 0.5f, 0);

		private Cube _cube;
		private CheckPointHolder _checkPointHolder;
		private CubeDissolveAnimation _cubeDissolveAnimation;
		private CubeStateHandler _cubeStateHandler;
		private TargetCameraHolder _targetCameraHolder;

		[Inject]
		private void Inject(CheckPointHolder checkPointHolder, TargetCameraHolder targetCameraHolder, Cube cube)
		{
			_targetCameraHolder = targetCameraHolder;
			_checkPointHolder = checkPointHolder;

			_cube = cube;
			_cubeDissolveAnimation = _cube.ServiceHolder.DissolveAnimation;
			_cubeStateHandler = _cube.ServiceHolder.StateHandler;

			_cube.Died += OnDied;
		}

		private void OnDisable() =>
			_cube.Died -= OnDied;

		private void OnDied()
		{
			if (_cubeStateHandler.CurrentState == CubeState.Falling)
			{
				DissolveVisible();
			}
			else
			{
				DissolveInvisible();
			}
		}

		private void DissolveInvisible() =>
			_cubeDissolveAnimation.Play(false, DissolveVisible);

		private void DissolveVisible()
		{
			_cube.transform.position = _checkPointHolder.CurrentCheckPoint.transform.position + _offset;

			_targetCameraHolder.SetLookAt();

			_cubeDissolveAnimation.Play(true, () =>
			{
				_cubeStateHandler.EnterIn(CubeState.Normal);
			});
		}
	}
}