using UnityEngine;

namespace LeadTools.Other
{
	public class ObjectFollower : MonoBehaviour
	{
		[SerializeField] private Transform _target;
		[SerializeField] private Vector3 _offset;

		private void Update()
		{
			if (_target == null)
			{
				return;
			}
			
			transform.position = _target.position + _offset;
		}

		public void SetTarget(Transform target)
		{
			if (target == null || _target == target)
			{
				return;
			}

			_target = target;
		}
	}
}