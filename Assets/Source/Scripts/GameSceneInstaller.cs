using System;
using System.Collections.Generic;
using Cinemachine;
using CubeProject.InputSystem;
using CubeProject.PlayableCube;
using CubeProject.SO;
using CubeProject.Tips;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Core;
using Source.Scripts.Game;
using Source.Scripts.Game.Level.Camera;
using Source.Scripts.Game.Level.Shield;
using Source.Scripts.Game.Level.Shield.States;
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

			var targetCameraHolder = new TargetCameraHolder(_virtualCamera);

			var shieldStateMachine = new ShieldStateMachine(
				() => new Dictionary<Type, State<ShieldStateMachine>>
				{
					[typeof(PlayState)] = new PlayState(),
					[typeof(StopState)] = new StopState(),
				});

			containerBuilder.AddSingleton(shieldStateMachine, typeof(IStateChangeable<ShieldStateMachine>));

			playerInitializer.Init(InputService, _maskHolder, shieldStateMachine);

			InitTargetCameraHolder();
			InitInput();

			_portalColorData.ResetColors();
			containerBuilder.AddSingleton(_portalColorData);

			containerBuilder.AddSingleton(playerInitializer.SpawnPoint);

			containerBuilder.AddSingleton(playerInitializer.Cube.Component, typeof(CubeComponent));

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