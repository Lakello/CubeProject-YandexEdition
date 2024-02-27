using System;
using LeadTools.StateMachine;
using UnityEngine;

namespace CubeProject.UI
{
	public abstract class EventTriggerButton : MonoBehaviour, ISubject
	{
		public event Action ActionEnded;

		public virtual void OnClick() =>
			ActionEnded?.Invoke();
	}
}