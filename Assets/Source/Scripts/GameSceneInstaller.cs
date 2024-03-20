using System;
using System.Collections.Generic;
using Cinemachine;
using CubeProject.Game;
using CubeProject.InputSystem;
using CubeProject.PlayableCube;
using CubeProject.Tips;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Reflex.Core;
using Source.Scripts.Game;
using Source.Scripts.Game.Level;
using Source.Scripts.Game.Level.Camera;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject
{
	public class GameSceneInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private MaskHolder _maskHolder;
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		[SerializeField] private CheckPointHolder _checkPointHolder;
		[SerializeField] private SpawnPoint _spawnPoint;
		[SerializeField] private Player _playerPrefab;
		[SerializeField] private bool _isMobileTest;

		private Action _disable;
		
		private void OnDisable() =>
			_disable?.Invoke();

		public void InstallBindings(ContainerDescriptor descriptor)
		{
			Player playerInstance;
			Cube cube;
			CubeStateMachine cubeStateMachine;
			
			InitCube();
			
			InitInput();
			
			descriptor.AddSingleton(cube);
			
			_checkPointHolder.Init(_spawnPoint);
			descriptor.AddSingleton(_checkPointHolder);

			descriptor.AddSingleton(_virtualCamera);

			descriptor.AddSingleton(new PushStateHandler(cube));

			descriptor.AddSingleton(_maskHolder);
			
			descriptor.AddSingleton(new TargetCameraHolder(
				this,
				_virtualCamera,
				playerInstance.Cube.transform,
				playerInstance.Follower));
			
			return;

			void InitCube()
			{
				playerInstance = Instantiate(
					_playerPrefab,
					_spawnPoint.transform.position,
					Quaternion.identity);

				cubeStateMachine = new CubeStateMachine(
					() => new Dictionary<Type, State<CubeStateMachine>>
					{
						[typeof(ControlState)] = new ControlState(),
						[typeof(DieState)] = new DieState(),
						[typeof(FallingToAbyssState)] = new FallingToAbyssState(),
						[typeof(FallingToGroundState)] = new FallingToGroundState(),
						[typeof(PushState)] = new PushState(),
						[typeof(TeleportState)] = new TeleportState(),
					});
				
				cube = playerInstance.Cube;
				cube.ServiceHolder.Init(cubeStateMachine);
				
				cubeStateMachine.EnterIn<ControlState>();
			}
			
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
				
				inputService.Init(playerInput, cubeStateMachine);				

				descriptor.AddSingleton(inputService, typeof(IInputService));
			}
		}
	}
}