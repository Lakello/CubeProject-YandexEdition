using System;
using Cinemachine;
using CubeProject.Game;
using CubeProject.InputSystem;
using CubeProject.Player;
using CubeProject.Tips;
using LeadTools.Extensions;
using Reflex.Core;
using Source.Scripts.Game;
using UnityEngine;

namespace CubeProject
{
	public class GameSceneInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private MaskHolder _maskHolder;
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		[SerializeField] private CheckPointHolder _checkPointHolder;
		[SerializeField] private Cube _cube;
		[SerializeField] private bool _isMobileTest;

		private Action _disabling;
		
		private void OnDisable() =>
			_disabling?.Invoke();

		public void InstallBindings(ContainerDescriptor descriptor)
		{
			InitInput();
			
			descriptor.AddSingleton(_cube);
			descriptor.AddSingleton(_checkPointHolder);

			descriptor.AddSingleton(_virtualCamera);

			descriptor.AddSingleton(new PushStateHandler(_cube));

			descriptor.AddSingleton(_maskHolder);
			
			return;

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
				
				_cube.gameObject.GetComponentElseThrow(out CubeStateHandler cubeStateHandler);
				inputService.Init(playerInput, cubeStateHandler);				

				descriptor.AddSingleton(inputService, typeof(IInputService));
			}
		}
	}
}