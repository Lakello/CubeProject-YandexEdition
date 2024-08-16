using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace CubeProject.Game.Level.Trigger
{
	public class TriggerDetector : MonoBehaviour
	{
		[SerializeField] private List<TriggerTarget> _targets;
		[SerializeField] [ReadOnly] private Transform _targetTransforms;
		[SerializeField] [ReadOnly] private bool _canGetTargetTransform;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out TriggerTarget target))
				_targets.Add(target);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out TriggerTarget target))
				_targets.Remove(target);
		}
		
		public bool TryGetTargetTransform([CanBeNull] out Transform targetTransform)
		{
			targetTransform = null;
			
			if (_targets.Count == 0)
				return false;
			
			if (_targets.Count == 1)
			{
				if (_targets[0].Chargeable.IsCharged)
				{
					targetTransform = _targets[0].transform;

					return true;
				}
			}

			var chargedTargets = _targets.Where(target => target.Chargeable.IsCharged).ToArray();
			
			return targetTransform = FindMinDistanceTransform(chargedTargets);
		}

		[Button]
		private void ShowTransforms()
		{
			_canGetTargetTransform = TryGetTargetTransform(out _targetTransforms);
		}

		private Transform FindMinDistanceTransform(TriggerTarget[] chargedTargets)
		{
			if (chargedTargets.Length == 0)
				return null;

			if (chargedTargets.Length == 1)
				return chargedTargets[0].transform;
			
			Transform result = chargedTargets[0].transform;

			for (int i = 0; i < chargedTargets.Length; i++)
			{
				if (Vector3.Distance(chargedTargets[i].transform.position, transform.position)
					< Vector3.Distance(result.position, transform.position))
				{
					result = chargedTargets[i].transform;
				}
			}

			return result;
		}
	}
}