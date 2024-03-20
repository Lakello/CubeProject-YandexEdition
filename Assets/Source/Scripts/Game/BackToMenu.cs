using System;
using LeadTools.StateMachine;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public class BackToMenu : MonoBehaviour, ITransitSubject
	{
		public event Action StateTransiting;

		[Inject]
		private void Inject(IInputService input) =>
			input.MenuKeyChanged += () => StateTransiting?.Invoke();
	}
}