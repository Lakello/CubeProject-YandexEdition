using CubeProject.Game;
using UnityEngine;

namespace Source.Scripts.Game.Level.Trigger
{
	public class TriggerObserver : MonoBehaviour
	{
		public Transform CurrentTarget { get; private set; }
		public IChargeable Chargeable { get; private set; }

		public void Entered(Transform target)
		{
			CurrentTarget = target;

			if (CurrentTarget.TryGetComponent(out IChargeable chargeable))
				Chargeable = chargeable;
		}

		public void Exited(Transform target)
		{
			if (CurrentTarget == target)
			{
				CurrentTarget = null;
				Chargeable = null;
			}
		}
	}
}