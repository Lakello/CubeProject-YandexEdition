using UnityEngine;

namespace Source.Scripts.Game.Level.Trigger
{
	public class TriggerObserver : MonoBehaviour
	{
		public Transform CurrentTarget { get; private set; }

		public void Entered(Transform target)
		{
			CurrentTarget = target;
		}

		public void Exited(Transform target)
		{
			if (CurrentTarget == target)
			{
				CurrentTarget = null;
			}
		}
	}
}