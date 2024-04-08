using UnityEngine;

namespace Source.Scripts.Game.Level.Trigger
{
	[RequireComponent(typeof(BoxCollider))]
	public class TriggerBehaviour : MonoBehaviour
	{
		[SerializeField] private Transform _target;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out TriggerObserver observer))
			{
				observer.Entered(_target);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out TriggerObserver observer))
			{
				observer.Exited(_target);
			}
		}
	}
}