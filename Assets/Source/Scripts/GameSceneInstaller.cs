using System;
using Cinemachine;
using CubeProject.Game;
using CubeProject.InputSystem;
using CubeProject.PlayableCube;
using CubeProject.Tips;
using LeadTools.Extensions;
using Reflex.Core;
using Source.Scripts.Game;
using Source.Scripts.Game.Level;
using Source.Scripts.Game.Level.Camera;
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

		private Action _disabling;
		
		private void OnDisable() =>
			_disabling?.Invoke();

		public void InstallBindings(ContainerDescriptor descriptor)
		{
			Player playerInstance;
			Cube cube;
			InitCube();
			
			InitInput();
			
			descriptor.AddSingleton(cube);
			
			_checkPointHolder.Init(_spawnPoint);
			descriptor.AddSingleton(_checkPointHolder);

			descriptor.AddSingleton(_virtualCamera);

			descriptor.AddSingleton(new PushStateHandler(cube));

			descriptor.AddSingleton(_maskHolder);
			
			descriptor.AddSingleton(new TargetCameraHolder(this, _virtualCamera, cube, playerInstance.Follow));
			
			return;

			void InitCube()
			{
				playerInstance = Instantiate(
					_playerPrefab,
					_spawnPoint.transform.position,
					Quaternion.identity);
				
				cube = playerInstance.Cube;
			}
			
			void InitInput()
			{
				var playerInput = new PlayerInput();

				playerInput.Enable();
				_disabling += () => playerInput.Disable();
				
				IInputService inputService;

                if (Application.isMobilePlatform || _isMobileTest)
				{
					inputService = gameObject.AddComponent<MobileInputService>();					
				}
				else
				{
                    inputService = gameObject.AddComponent<DesktopInputService>();
                }
				
				cube.gameObject.GetComponentElseThrow(out CubeStateService cubeStateHandler);
				inputService.Init(playerInput, cubeStateHandler);				

				descriptor.AddSingleton(inputService, typeof(IInputService));
			}
		}
	}
}