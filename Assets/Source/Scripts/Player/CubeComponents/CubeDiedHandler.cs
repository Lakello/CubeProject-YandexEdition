using Cinemachine;
using CubeProject.Game;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Player
{
	public class CubeDiedHandler : MonoBehaviour
	{
		[SerializeField] private Transform _cameraFollow;
		[SerializeField] private Vector3 _offset = new Vector3(0, 0.5f, 0);

		private Cube _cube;
		private CheckPointHolder _checkPointHolder;
		private CubeDissolveAnimation _cubeDissolveAnimation;
		private CubeStateHandler _cubeStateHandler;
		private CinemachineVirtualCamera _virtualCamera;

		[Inject]
		private void Inject(CheckPointHolder checkPointHolder, CinemachineVirtualCamera virtualCamera, Cube cube)
		{
			_checkPointHolder = checkPointHolder;
			_virtualCamera = virtualCamera;

			_cube = cube;
			_cubeDissolveAnimation = _cube.ComponentsHolder.DissolveAnimation;
			_cubeStateHandler = _cube.ComponentsHolder.StateHandler;

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

			_virtualCamera.Follow = _cameraFollow;
			_virtualCamera.LookAt = transform;

			_cubeDissolveAnimation.Play(true, () =>
			{
				_cubeStateHandler.EnterIn(CubeState.Normal);
			});
		}
	}
}