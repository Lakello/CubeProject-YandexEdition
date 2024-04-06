using System;
using Cinemachine;
using CubeProject.InputSystem;
using CubeProject.SO;
using CubeProject.Tips;
using LeadTools.Extensions;
using Reflex.Core;
using Source.Scripts.Game;
using Source.Scripts.Game.Level;
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
		[SerializeField] private bool _isMobileTest;

		private SpawnPoint _spawnPoint;
		private Action _disable;

		private void OnDisable() =>
			_disable?.Invoke();

		public void InstallBindings(ContainerDescriptor descriptor)
		{
			var playerInitializer = gameObject.GetComponentElseThrow<CubeInitializer>();

			playerInitializer.Init();

			InitInput();

			_portalColorData.ResetColorIndex();
			descriptor.AddSingleton(_portalColorData);

			descriptor.AddSingleton(playerInitializer.Cube);

			descriptor.AddSingleton(_spawnPoint);

			descriptor.AddSingleton(_virtualCamera);

			descriptor.AddSingleton(new PushStateHandler(playerInitializer.Cube));

			descriptor.AddSingleton(_maskHolder);

			descriptor.AddSingleton(new TargetCameraHolder(
				this,
				_virtualCamera,
				playerInitializer.PlayerInstance.Cube.transform,
				playerInitializer.PlayerInstance.Follower));

			return;

			void InitInput()
			{
				var playerInput = new PlayerInput();

				playerInput.Enable();
				_disable += () => playerInput.Disable();

				IInputService inputService;

				if (Application.isMobilePlatform || _isMobileTest)
				{
					inputService = gameObject.AddComponent<MobileInputService>();
				}
				else
				{
					inputService = gameObject.AddComponent<DesktopInputService>();
				}

				inputService.Init(playerInput, playerInitializer.CubeStateMachine);

				descriptor.AddSingleton(inputService, typeof(IInputService));
			}
		}
	}
}