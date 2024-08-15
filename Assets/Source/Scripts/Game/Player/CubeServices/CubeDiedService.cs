using System;
using CubeProject.Game.Level;
using CubeProject.Game.Messages;
using CubeProject.Game.Messages.Camera;
using CubeProject.Game.PlayerStateMachine;
using CubeProject.Game.PlayerStateMachine.States;
using LeadTools.StateMachine;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	public class CubeDiedService : IDisposable
	{
		private readonly Vector3 _offset = new Vector3(0, 0.5f, 0);
		private readonly CubeEntity _cubeEntity;
		private readonly CubeDiedView _cubeDiedView;
		private readonly SpawnPoint _spawnPoint;
		private readonly M_SetTargetCamera _setTargetCameraMessage = new M_SetTargetCamera();
		private readonly M_CheckGround _checkGroundMessage = new M_CheckGround();

		public CubeDiedService(CubeEntity cubeEntity, SpawnPoint spawnPoint)
		{
			_cubeEntity = cubeEntity;
			_cubeDiedView = _cubeEntity.Component.DiedView;
			_spawnPoint = spawnPoint;
			
			_cubeEntity.Died += OnDied;
		}

		private IStateMachine<CubeStateMachine> CubeStateMachine => _cubeEntity.Component.StateMachine;

		public void Dispose()
		{
			_cubeEntity.Died -= OnDied;
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
			_cubeEntity.transform.position = _spawnPoint.transform.position + _offset;
			
			MessageBroker.Default
				.Publish(_setTargetCameraMessage);

			_cubeDiedView.Play(true, () =>
			{
				MessageBroker.Default
					.Publish(_checkGroundMessage.SetData(isGrounded =>
					{
						if (isGrounded is false)
							CubeStateMachine.EnterIn<ControlState>();
					}));
			});
		}
	}
}