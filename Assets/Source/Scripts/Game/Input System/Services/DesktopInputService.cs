using System;
using System.Collections;
using CubeProject.Game.Player.FSM;
using CubeProject.Game.Player.FSM.States;
using CubeProject.InputSystem;
using LeadTools.FSM;
using UniRx;
using UnityEngine;

namespace CubeProject.Game.InputSystem
{
	public class DesktopInputService : IInputService
	{
		private readonly PlayerInput _playerInput;
		private readonly IStateChangeable<CubeStateMachine> _cubeStateMachine;

		private bool _isUpdateInput;

		public DesktopInputService(PlayerInput playerInput, IStateChangeable<CubeStateMachine> cubeStateMachine)
		{
			_playerInput = playerInput;

			_cubeStateMachine = cubeStateMachine;

			_cubeStateMachine.SubscribeTo<ControlState>(OnControlStateChanged);

			_playerInput.Desktop.Menu.performed += _ => OnMenuPerformed();

			OnControlStateChanged(_cubeStateMachine.CurrentState == typeof(ControlState));
		}

		public event Action<Vector3> Moving;

		public event Action MenuKeyChanged;

		public void Dispose()
		{
			_playerInput?.Dispose();
			_cubeStateMachine?.UnSubscribeTo<ControlState>(OnControlStateChanged);
		}

		private void OnControlStateChanged(bool isEntered)
		{
			if (isEntered)
			{
				_isUpdateInput = true;
				MainThreadDispatcher.StartFixedUpdateMicroCoroutine(UpdateInput());
			}
			else
			{
				_isUpdateInput = false;
			}
		}

		private void OnMenuPerformed() =>
			MenuKeyChanged?.Invoke();

		private IEnumerator UpdateInput()
		{
			float horizontal;
			float vertical;

			while (_isUpdateInput)
			{
				horizontal = _playerInput.Desktop.Horizontal.ReadValue<float>();
				vertical = _playerInput.Desktop.Vertical.ReadValue<float>();

				Moving?.Invoke(new Vector3(horizontal, 0, vertical));

				yield return null;
			}
		}
	}
}