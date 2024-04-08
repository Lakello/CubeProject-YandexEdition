using System;
using LeadTools.StateMachine;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public class BackToMenu : MonoBehaviour, ITransitSubject
	{
		private IInputService _inputService;

		public event Action StateTransiting;

		[Inject]
		private void Inject(IInputService inputService)
		{
			_inputService = inputService;

			_inputService.MenuKeyChanged += OnMenuKeyChanged;
		}

		private void OnDisable() =>
			_inputService.MenuKeyChanged -= OnMenuKeyChanged;

		private void OnMenuKeyChanged() =>
			StateTransiting?.Invoke();
	}
}