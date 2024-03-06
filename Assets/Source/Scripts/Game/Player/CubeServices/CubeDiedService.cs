using CubeProject.Game;
using Reflex.Attributes;
using Source.Scripts.Game.Level.Camera;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeDiedService : MonoBehaviour
	{
		[SerializeField] private Transform _cameraFollow;
		[SerializeField] private Vector3 _offset = new Vector3(0, 0.5f, 0);

		private Cube _cube;
		private CheckPointHolder _checkPointHolder;
		private CubeDiedView _cubeDiedView;
		private CubeStateService _cubeStateService;
		private TargetCameraHolder _targetCameraHolder;

		[Inject]
		private void Inject(CheckPointHolder checkPointHolder, TargetCameraHolder targetCameraHolder, Cube cube)
		{
			_targetCameraHolder = targetCameraHolder;
			_checkPointHolder = checkPointHolder;

			_cube = cube;
			_cubeDiedView = _cube.ServiceHolder.DiedView;
			_cubeStateService = _cube.ServiceHolder.StateService;

			_cube.Died += OnDied;
		}

		private void OnDisable() =>
			_cube.Died -= OnDied;

		private void OnDied()
		{
			if (_cubeStateService.CurrentState == CubeState.Falling)
			{
				DissolveVisible();
			}
			else
			{
				DissolveInvisible();
			}
		}

		private void DissolveInvisible() =>
			_cubeDiedView.Play(false, DissolveVisible);

		private void DissolveVisible()
		{
			_cube.transform.position = _checkPointHolder.CurrentCheckPoint.transform.position + _offset;

			_targetCameraHolder.SetLookAt();

			_cubeDiedView.Play(true, () =>
			{
				_cubeStateService.EnterIn(CubeState.Normal);
			});
		}
	}
}