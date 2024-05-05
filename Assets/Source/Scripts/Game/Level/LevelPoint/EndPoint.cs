using System;
using CubeProject.PlayableCube;
using LeadTools.StateMachine;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(Collider))]
	public class EndPoint : MonoBehaviour, ITransitSubject
	{
		public event Action StateTransiting;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube _))
				StateTransiting?.Invoke();
		}
	}
}