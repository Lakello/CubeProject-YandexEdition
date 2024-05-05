using System;
using LeadTools.StateMachine;

namespace CubeProject.InputSystem
{
	public class BackToMenuHandler : IDisposable, ITransitSubject
	{
		private readonly IInputService _inputService;

		public event Action StateTransiting;

		public BackToMenuHandler(IInputService inputService)
		{
			_inputService = inputService;

			_inputService.MenuKeyChanged += OnMenuKeyChanged;
		}

		public void Dispose() =>
			_inputService.MenuKeyChanged -= OnMenuKeyChanged;

		private void OnMenuKeyChanged() =>
			StateTransiting?.Invoke();
	}
}