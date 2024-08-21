using LeadTools.Object;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LeadTools.Other
{
	public class ObjectFollower : MonoBehaviour
	{
		[SerializeField] private FollowMode _mode;
		[SerializeField] private Transform _target;
		[SerializeField] private Vector3 _offset;
		[SerializeField] [ShowIf(nameof(CanShowMoveStepDuration))] [MinMaxSlider(0, 10)]
		private Vector2 _speedRange = new Vector2(0, 5f);
		[SerializeField] [ShowIf(nameof(CanShowMoveStepDuration))] [MinMaxSlider(0, 5)]
		private Vector2 _distanceRange = new Vector2(0.05f, 0.1f);

		private bool CanShowMoveStepDuration => _mode == FollowMode.Smooth;

		private Vector3 TargetPosition => _target.position + _offset;

		private void Update()
		{
			if (_target == null)
			{
				Debug.LogError($"Required Target", gameObject);

				return;
			}

			if (_mode == FollowMode.Hard)
				MoveHard();
			else if (_mode == FollowMode.Smooth)
				MoveSmooth();
		}

		private void MoveHard() =>
			transform.position = TargetPosition;

		private void MoveSmooth()
		{
			var direction = TargetPosition - transform.position;

			var speed = CalculateSpeed();

			transform.position += direction.normalized * (speed * Time.deltaTime);
		}

		private float CalculateSpeed()
		{
			var distance = Vector3.Distance(TargetPosition, transform.position);

			if (distance < _distanceRange.x)
				return 0;

			var normalDistance = Mathf.Lerp(0, 1, distance / _distanceRange.y);

			return Mathf.LerpUnclamped(_speedRange.x, _speedRange.y, normalDistance);
		}
	}
}