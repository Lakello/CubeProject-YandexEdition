using System;
using System.Collections;
using LeadTools.Extensions;
using LeadTools.StateMachine;
using Source.Scripts.Game.tateMachine;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public class MobileInputService : MonoBehaviour, IInputService
	{
		private PlayerInput _playerInput;
		private Coroutine _updateInputCoroutine;
		private IStateChangeable<CubeStateMachine> _cubeStateMachine;

		public event Action<Vector3> Moving;
		public event Action MenuKeyChanged;

		public void Init(PlayerInput playerInput, IStateChangeable<CubeStateMachine> cubeStateMachine)
		{
			_playerInput = playerInput;
			_cubeStateMachine = cubeStateMachine;

			_cubeStateMachine.SubscribeTo<ControlState>(OnControlStateChanged);

			OnControlStateChanged(_cubeStateMachine.CurrentState == typeof(ControlState));
		}

		private void OnDisable()
		{
			if (_cubeStateMachine != null)
			{
				_cubeStateMachine.UnSubscribeTo<ControlState>(OnControlStateChanged);
			}
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

		private IEnumerator UpdateInput()
		{
			var wait = new WaitForFixedUpdate();

			Vector2 input;
			float horizontal;
			float vertical;

			while (enabled)
			{
				input = _playerInput.Mobile.Delta.ReadValue<Vector2>();

				input = input.normalized;

				horizontal = input.x;
				vertical = input.y;

				// if (horizontal > vertical)
				// {
				//     ToInt(ref horizontal);
				//     vertical = 0;
				// }
				// else if (horizontal == vertical)
				// {
				//     horizontal = 0;
				//     vertical = 0;
				// }
				// else
				// {
				//     horizontal = 0;
				//     ToInt(ref vertical);
				// }

				Moving?.Invoke(new Vector3(horizontal, 0, vertical));

				yield return wait;
			}
		}

		// TODO КАКА!!! Поменять
		// private void ToInt(ref float value)
		// {
		//     if (value > 0)
		//     {
		//         value = 1;
		//     }
		//     else if (value < 0)
		//     {
		//         value = -1;
		//     }
		//     else
		//     {
		//         value = 0;
		//     }
		// }
	}
}