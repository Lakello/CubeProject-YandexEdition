using System;
using Cinemachine;
using CubeProject.InputSystem;
using CubeProject.SO;
using CubeProject.Tips;
using LeadTools.Extensions;
using Reflex.Core;
using Source.Scripts.Game;
using Source.Scripts.Game.Level.Camera;
using UnityEngine;

namespace CubeProject
{
	[RequireComponent(typeof(CubeInitializer))]
	public class GameSceneInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private MaskHolder _maskHolder;
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		[SerializeField] private PortalColorData _portalColorData;

		private Action _disable;
		private IInputService _inputService;
		private PlayerInput _playerInput;

		public IInputService InputService => _inputService ??= TryCreateInputService();
		
		private void OnDisable() =>
			_disable?.Invoke();

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			var playerInitializer = gameObject.GetComponentElseThrow<CubeInitializer>();

			var targetCameraHolder = new TargetCameraHolder(this, _virtualCamera);
			
			playerInitializer.Init(InputService, _maskHolder, targetCameraHolder);
			
			InitTargetCameraHolder();
			InitInput();

			_portalColorData.ResetColorIndex();
			containerBuilder.AddSingleton(_portalColorData);

			containerBuilder.AddSingleton(playerInitializer.Cube);

			containerBuilder.AddSingleton(playerInitializer.SpawnPoint);

			containerBuilder.AddSingleton(_virtualCamera);

			containerBuilder.AddSingleton(typeof(PushStateHandler));

			containerBuilder.AddSingleton(_maskHolder);

			containerBuilder.AddSingleton(targetCameraHolder);

			return;

			void InitInput()
			{
				InputService.Init(_playerInput, playerInitializer.CubeStateMachine);

				containerBuilder.AddSingleton(InputService, typeof(IInputService));
			}

			void InitTargetCameraHolder()
			{
				targetCameraHolder.Init(
					playerInitializer.PlayerInstance.Cube.transform, 
					playerInitializer.PlayerInstance.Follower);
				targetCameraHolder.SetTarget();
			}
		}

		private IInputService TryCreateInputService()
		{
			if (_inputService == null)
			{
				_playerInput = new PlayerInput();

				_playerInput.Enable();
				_disable += () => _playerInput.Disable();

				_inputService = gameObject.AddComponent<DesktopInputService>();
			}

			return _inputService;
		}
	}
}