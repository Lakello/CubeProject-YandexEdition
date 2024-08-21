using System;
using CubeProject.Game.InputSystem;
using LeadTools.FSM.Transit;

namespace CubeProject.Game
{
	public class BackToMenuHandler : IDisposable, ITransitSubject
	{
		private readonly IInputService _inputService;

		public BackToMenuHandler(IInputService inputService)
		{
			_inputService = inputService;

			_inputService.MenuKeyChanged += OnMenuKeyChanged;
		}

		public event Action StateTransiting;

		public void Dispose() =>
			_inputService.MenuKeyChanged -= OnMenuKeyChanged;

		private void OnMenuKeyChanged() =>
			StateTransiting?.Invoke();
	}
}