using System;
using Cinemachine;
using CubeProject.InputSystem;
using CubeProject.SO;
using CubeProject.Tips;
using LeadTools.StateMachine;
using Reflex.Core;
using Sirenix.Utilities;
using CubeProject.Game.Player;
using CubeProject.Game.Level;
using CubeProject.Game.Level.Camera;
using CubeProject.Game.Player.Shield;
using UnityEngine;

namespace CubeProject
{
	public class GameSceneInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private CubeData _cubeData;
		[SerializeField] private MaskHolder _maskHolder;
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		[SerializeField] private PortalColorData _portalColorData;

		private Action _disable;
		private IDisposable[] _disposables;
		private PlayerInput _playerInput;

		public IInputService InputService { get; private set; }

		private void OnDisable()
		{
			_disable?.Invoke();
			_disposables.ForEach(disposable => disposable.Dispose());
		}

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			var cubeFactory = new CubeFactory(_cubeData, FindObjectOfType<SpawnPoint>());

			InputService = InitInputService();
			containerBuilder.AddSingleton(InputService, typeof(IInputService));

			cubeFactory.Create(InputService, _maskHolder);

			_disposables = new IDisposable[]
			{
				InputService, cubeFactory
			};

			containerBuilder.AddSingleton(
				cubeFactory.ShieldStateMachine,
				typeof(IStateChangeable<ShieldStateMachine>));

			containerBuilder.AddSingleton(cubeFactory.CubeEntity.Component);

			_portalColorData.ResetColors();
			containerBuilder.AddSingleton(_portalColorData);

			containerBuilder.AddSingleton(_virtualCamera);

			containerBuilder.AddSingleton(typeof(PushStateHandler));

			containerBuilder.AddSingleton(_maskHolder);

			containerBuilder.AddSingleton(InitTargetCameraHolder());

			return;

			TargetCameraHolder InitTargetCameraHolder() =>
				new TargetCameraHolder(
					_virtualCamera,
					cubeFactory.PlayerEntityInstance.CubeEntity.transform,
					cubeFactory.PlayerEntityInstance.Follower);

			IInputService InitInputService()
			{
				_playerInput = new PlayerInput();

				_playerInput.Enable();
				_disable += () => _playerInput.Disable();

				return new DesktopInputService(
					_playerInput,
					cubeFactory.CubeStateMachine);
			}
		}
	}
}