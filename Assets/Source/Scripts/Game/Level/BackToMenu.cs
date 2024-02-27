using System;
using LeadTools.StateMachine;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public class BackToMenu : MonoBehaviour, ISubject
	{
		public event Action ActionEnded;

		[Inject]
		private void Inject(IInputService input) =>
			input.MenuKeyChanged += () => ActionEnded?.Invoke();
	}
}