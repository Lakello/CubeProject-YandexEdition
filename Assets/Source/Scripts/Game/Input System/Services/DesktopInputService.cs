using System;
using System.Collections;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public class DesktopInputService : MonoBehaviour, IInputService
	{
		private PlayerInput _playerInput;
		private Coroutine _updateInputCoroutine;
		private CubeStateService _stateService;

		public event Action<Vector3> Moving;

		public event Action UsePressed;

		public event Action UseReleased;

		public event Action MenuKeyChanged;

		private void OnDisable()
		{
			if (_stateService != null)
			{
				_stateService.StateChanged -= OnStateChanged;
			}
		}

		public void Init(PlayerInput playerInput, CubeStateService stateService)
		{
			_playerInput = playerInput;

			_stateService = stateService;

			_stateService.StateChanged += OnStateChanged;

			_playerInput.Desktop.UsePress.performed += _ => OnUsePressPerformed();
			_playerInput.Desktop.UseRelease.performed += _ => OnUseReleasePerformed();

			_playerInput.Desktop.Menu.performed += _ => OnMenuPerformed();

			OnStateChanged(_stateService.CurrentState);
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

		private void OnMenuPerformed() =>
			MenuKeyChanged?.Invoke();

		private void OnUsePressPerformed() =>
			UsePressed?.Invoke();

		private void OnUseReleasePerformed() =>
			UseReleased?.Invoke();

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