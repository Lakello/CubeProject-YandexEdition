using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public class DoorHandler : MonoBehaviour
	{
		[SerializeField] private Door[] _doors;
		[SerializeField] private float _animationTime;

		private Coroutine _doorCoroutine;
		private bool _previousCharged;
		private ChargeConsumer _chargeConsumer;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeConsumer);
		}

		private void OnEnable()
		{
			_chargeConsumer.ChargeChanged += OnChargeChanged;
			OnChargeChanged();
		}

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged()
		{
			var currentCharged = _chargeConsumer.IsCharged;

			if (_previousCharged == currentCharged)
			{
				return;
			}

			_previousCharged = currentCharged;

			this.StopRoutine(_doorCoroutine);

			ChangeState(currentCharged);
		}

		private void ChangeState(bool isCharged)
		{
			foreach (var door in _doors)
			{
				door.StartScale = door.transform.localScale;
				door.StartPosition = door.transform.localPosition;
			}

			_doorCoroutine = this.PlaySmoothChangeValue(
				(currentTime) =>
				{
					foreach (var door in _doors)
					{
						var doorTransform = door.transform;

						var (position, scale) = CalculateDoorData(door, currentTime);

						doorTransform.localPosition = position;
						doorTransform.localScale = scale;
					}
				},
				_animationTime);

			return;

			(Vector3, Vector3) CalculateDoorData(Door door, float currentTime)
			{
				var (targetPosition, targetScale) = isCharged
					? (door.OpenPosition, door.OpenScale)
					: (door.ClosePosition, door.CloseScale);

				var position = Vector3.Lerp(door.StartPosition, targetPosition, currentTime);
				var scale = Vector3.Lerp(door.StartScale, targetScale, currentTime);

				return (position, scale);
			}
		}
	}
}