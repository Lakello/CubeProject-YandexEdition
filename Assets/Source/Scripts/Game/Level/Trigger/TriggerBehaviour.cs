using System;
using UnityEngine;

namespace Source.Scripts.Game.Level.Trigger
{
	public class TriggerBehaviour : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out TriggerObserver observer))
			{
				observer.Entered(transform);
			}
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out TriggerObserver observer))
			{
				observer.Exited(transform);
			}
		}
	}
}