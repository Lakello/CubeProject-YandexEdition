using System;
using Game.Player;
using LeadTools.StateMachine;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(Collider))]
	public class EndPoint : MonoBehaviour, ITransitSubject
	{
		public event Action StateTransiting;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CubeEntity _))
				StateTransiting?.Invoke();
		}
	}
}