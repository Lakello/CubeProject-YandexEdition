using System;
using System.Collections;
using CubeProject.PlayableCube;
using CubeProject.Player;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public class MobileInputService : MonoBehaviour, IInputService
	{
		private PlayerInput _playerInput;
		private Coroutine _updateInputCoroutine;
		private CubeStateHandler _stateHandler;

		public event Action<Vector3> Moving;

		public event Action UsePressed;

		public event Action UseReleased;

		public event Action MenuKeyChanged;

		public void Init(PlayerInput playerInput, CubeStateHandler stateHandler)
		{
			_playerInput = playerInput;
			_stateHandler = stateHandler;

			_stateHandler.StateChanged += OnStateChanged;

			OnStateChanged(_stateHandler.CurrentState);
		}

		private void OnDisable()
		{
			if (_stateHandler != null)
			{
				_stateHandler.StateChanged -= OnStateChanged;
			}
		}

		private void OnStateChanged(CubeState state)
		{
			if (state == CubeState.Normal)
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