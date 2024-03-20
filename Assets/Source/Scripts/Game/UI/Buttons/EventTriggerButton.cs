using System;
using LeadTools.StateMachine;
using UnityEngine;

namespace CubeProject.UI
{
	public abstract class EventTriggerButton : MonoBehaviour, ITransitSubject
	{
		public event Action StateTransiting;

		public virtual void OnClick() =>
			StateTransiting?.Invoke();
	}
}