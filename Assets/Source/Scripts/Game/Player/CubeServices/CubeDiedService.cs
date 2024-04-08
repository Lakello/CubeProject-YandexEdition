using System;
using LeadTools.StateMachine;
using Source.Scripts.Game.Level;
using Source.Scripts.Game.Level.Camera;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeDiedService : IDisposable
	{
		private readonly Vector3 _offset = new Vector3(0, 0.5f, 0);
		private readonly Cube _cube;
		private readonly CubeDiedView _cubeDiedView;
		private readonly SpawnPoint _spawnPoint;
		private readonly TargetCameraHolder _targetCameraHolder;

		public CubeDiedService(Cube cube, SpawnPoint spawnPoint, TargetCameraHolder targetCameraHolder)
		{
			_cube = cube;
			_cubeDiedView = _cube.Component.DiedView;
			_targetCameraHolder = targetCameraHolder;
			_spawnPoint = spawnPoint;

			_cube.Died += OnDied;
		}

		private IStateMachine<CubeStateMachine> CubeStateMachine => _cube.Component.StateMachine;

		private CubeFallService CubeFallService => _cube.Component.FallService;

		public void Dispose()
		{
			_cube.Died -= OnDied;
		}

		private void OnDied()
		{
			if (CubeStateMachine.CurrentState == typeof(DieState))
			{
				DissolveInvisible();
			}
		}

		private void DissolveInvisible() =>
			_cubeDiedView.Play(false, DissolveVisible);

		private void DissolveVisible()
		{
			_cube.transform.position = _spawnPoint.transform.position + _offset;

			_targetCameraHolder.SetTarget();

			_cubeDiedView.Play(true, () =>
			{
				if (CubeFallService.TryFall() is false)
				{
					CubeStateMachine.EnterIn<ControlState>();
				}
			});
		}
	}
}