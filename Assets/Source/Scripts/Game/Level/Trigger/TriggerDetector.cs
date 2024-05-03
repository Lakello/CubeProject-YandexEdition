using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Source.Scripts.Game.Level.Trigger
{
	public class TriggerDetector : MonoBehaviour
	{
		[SerializeField] private List<TriggerTarget> _targets = new List<TriggerTarget>();

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out TriggerTarget target))
			{
				_targets.Add(target);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out TriggerTarget target))
			{
				_targets.Remove(target);
			}
		}

		public bool TryGetTargetTransform([CanBeNull] out Transform targetTransform)
		{
			targetTransform = _targets.FirstOrDefault(trigger => trigger.Chargeable.IsCharged)?.transform;

			return targetTransform;
		}
	}
}