using System;
using System.Collections;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public class DesktopInputService : MonoBehaviour, IInputService
	{
		private PlayerInput _playerInput;
		private Coroutine _updateInputCoroutine;
		private IStateChangeable<CubeStateMachine> _cubeStateMachine;

		public event Action<Vector3> Moving;

		public event Action MenuKeyChanged;

		private void OnDisable()
		{
			if (_cubeStateMachine != null)
			{
				_cubeStateMachine.UnSubscribeTo<ControlState>(OnControlStateChanged);
			}
		}

		public void Init(PlayerInput playerInput, IStateChangeable<CubeStateMachine> cubeStateMachine)
		{
			_playerInput = playerInput;

			_cubeStateMachine = cubeStateMachine;

			_cubeStateMachine.SubscribeTo<ControlState>(OnControlStateChanged);

			_playerInput.Desktop.Menu.performed += _ => OnMenuPerformed();
			
			OnControlStateChanged(_cubeStateMachine.CurrentState == typeof(ControlState));
		}

		private void OnControlStateChanged(bool isEntered)
		{
			if (isEntered)
			{
				_updateInputCoroutine = StartCoroutine(UpdateInput());
			}
			else
			{
				this.StopRoutine(_updateInputCoroutine);
			}
		}

		private void OnMenuPerformed() =>
			MenuKeyChanged?.Invoke();
		
		private IEnumerator UpdateInput()
		{
			var wait = new WaitForFixedUpdate();

			float horizontal;
			float vertical;

			while (enabled)
			{
				horizontal = _playerInput.Desktop.Horizontal.ReadValue<float>();
				vertical = _playerInput.Desktop.Vertical.ReadValue<float>();

				Moving?.Invoke(new Vector3(horizontal, 0, vertical));

				yield return wait;
			}
		}
	}
}