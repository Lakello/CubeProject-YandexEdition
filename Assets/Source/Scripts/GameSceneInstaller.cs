using System;
using Cinemachine;
using CubeProject.Game;
using CubeProject.InputSystem;
using CubeProject.Player;
using CubeProject.Tips;
using LeadTools.Extensions;
using Reflex.Core;
using UnityEngine;

namespace CubeProject
{
	public class GameSceneInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private UseTipKeyHandler _useTipKeyHandler;
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

			descriptor.AddSingleton(_useTipKeyHandler);

			descriptor.AddSingleton(new PusherStateHandler(_cube.ComponentsHolder.StateHandler));
			
			return;

			void InitInput()
			{
				var playerInput = new PlayerInput();

				playerInput.Enable();
				_disabling += () => playerInput.Disable();
				IInputHandler inputHandler;

                if (Application.isMobilePlatform || _isMobileTest)
				{
					inputHandler = gameObject.AddComponent<MobileInputHandler>();					
				}
				else
				{
                    inputHandler = gameObject.AddComponent<DesktopInputHandler>();
                }
				
				_cube.gameObject.GetComponentElseThrow(out CubeStateHandler cubeStateHandler);
				inputHandler.Init(playerInput, cubeStateHandler);				

				descriptor.AddSingleton(inputHandler, typeof(IInputHandler));
			}
		}
	}
}