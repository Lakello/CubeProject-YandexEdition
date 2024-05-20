using System;
using LeadTools.StateMachine;
using Source.Scripts.Game.Level;
using Source.Scripts.Game.Level.Camera;
using Source.Scripts.Game.Messages;
using Source.Scripts.Game.Messages.Camera;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UniRx;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeDiedService : IDisposable
	{
		private readonly Vector3 _offset = new Vector3(0, 0.5f, 0);
		private readonly Cube _cube;
		private readonly CubeDiedView _cubeDiedView;
		private readonly SpawnPoint _spawnPoint;

		public CubeDiedService(Cube cube, SpawnPoint spawnPoint)
		{
			_cube = cube;
			_cubeDiedView = _cube.Component.DiedView;
			_spawnPoint = spawnPoint;

			_cube.Died += OnDied;
		}

		private IStateMachine<CubeStateMachine> CubeStateMachine => _cube.Component.StateMachine;

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

			MessageBroker.Default
				.Publish(new SetTargetCameraMessage());

			_cubeDiedView.Play(true, () =>
			{
				MessageBroker.Default
					.Publish(new CheckGroundMessage(isGrounded =>
					{
						if (isGrounded is false)
							CubeStateMachine.EnterIn<ControlState>();
					}));
			});
		}
	}
}