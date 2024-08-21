using System;
using LeadTools.FSM.Transit;
using UnityEngine;

namespace CubeProject.Game.UI.Buttons
{
	public abstract class EventTriggerButton : MonoBehaviour, ITransitSubject
	{
		public event Action StateTransiting;

		public virtual void OnClick() =>
			StateTransiting?.Invoke();
	}
}